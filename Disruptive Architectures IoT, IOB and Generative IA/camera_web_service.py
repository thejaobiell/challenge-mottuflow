"""
Camera Web Service (Flask) para integrar a captura de câmera (OpenCV) com o frontend.

Funcionalidades:
- /ui       : página simples com botões Start/Stop e preview ao vivo
- /start    : inicia captura e detecção de ArUco (envia para API Java com JWT)
- /stop     : para a captura
- /status   : status JSON
- /stream   : stream MJPEG do último frame processado

Requisitos: pip install -r requirements.txt (inclui Flask, OpenCV, requests)
"""

import os
import time
import threading
from datetime import datetime
from typing import Optional, Tuple

import cv2
import numpy as np
import requests
from flask import Flask, Response, jsonify, request, render_template_string

try:
    from flask_cors import CORS
except Exception:
    CORS = None

# ===================== CONFIG =====================
JAVA_BASE = "http://localhost:8080"
LOGIN_URL = f"{JAVA_BASE}/api/login"
ARUCO_POST_URL = f"{JAVA_BASE}/api/aruco-tags/cadastrar"

ADMIN_EMAIL = "admin@email.com"
ADMIN_SENHA = "adminmottu"

API_TIMEOUT = 8
DEFAULT_CAMERA_ID = 0
ENVIO_INTERVALO = 2.0  # seg entre envios da mesma tag

# ArUco
# ArUco dictionaries suportados (nome -> constante)
DICT_MAP = {
    "DICT_6X6_250": cv2.aruco.DICT_6X6_250,
    "DICT_5X5_100": cv2.aruco.DICT_5X5_100,
    "DICT_4X4_50": cv2.aruco.DICT_4X4_50,
}

CURRENT_DICT_NAME = os.environ.get("ARUCO_DICT", "DICT_6X6_250")


def build_detector(dict_name: str):
    """Cria o detector com parâmetros otimizados e retorna (detector, params, dictionary)."""
    dict_name = dict_name if dict_name in DICT_MAP else "DICT_6X6_250"
    dictionary = cv2.aruco.getPredefinedDictionary(DICT_MAP[dict_name])
    params = cv2.aruco.DetectorParameters()
    # Afinando parâmetros para melhorar detecção em cenários de baixa luz/ruído
    params.adaptiveThreshWinSizeMin = 3
    params.adaptiveThreshWinSizeMax = 53
    params.adaptiveThreshWinSizeStep = 4
    params.adaptiveThreshConstant = 7
    params.minMarkerPerimeterRate = 0.02  # aceitar marcadores menores
    params.maxMarkerPerimeterRate = 4.0
    params.polygonalApproxAccuracyRate = 0.03
    params.minCornerDistanceRate = 0.05
    params.minDistanceToBorder = 1
    params.cornerRefinementMethod = cv2.aruco.CORNER_REFINE_SUBPIX  # melhora precisão
    detector = cv2.aruco.ArucoDetector(dictionary, params)
    return detector, params, dictionary


ARUCO_DETECTOR, ARUCO_PARAMS, ARUCO_DICT = build_detector(CURRENT_DICT_NAME)

# Pose estimation (básico)
REAL_MARKER_SIZE_M = 0.05
CAMERA_MATRIX = np.array([[1000, 0, 640], [0, 1000, 360], [0, 0, 1]], dtype=np.float32)
DIST_COEFFS = np.zeros((4, 1), dtype=np.float32)


app = Flask(__name__)
if CORS:
    CORS(
        app,
        resources={
            r"/*": {
                "origins": [
                    "http://localhost:8080",
                    "http://127.0.0.1:8080",
                    "http://localhost:*",
                    "http://127.0.0.1:*",
                ]
            }
        },
        supports_credentials=False,
    )

# Estado global simples
running = False
capture_thread: Optional[threading.Thread] = None
latest_frame = None
frame_lock = threading.Lock()
last_sent = {}  # tag_id -> timestamp
jwt_token: Optional[str] = None
selected_camera_id = DEFAULT_CAMERA_ID  # câmera atual em uso


def login_java() -> Optional[str]:
    try:
        r = requests.post(
            LOGIN_URL,
            json={"email": ADMIN_EMAIL, "senha": ADMIN_SENHA},
            timeout=API_TIMEOUT,
        )
        if r.ok:
            data = r.json()
            return data.get("tokenAcesso")
        else:
            print(f"[PY CAM] Falha login Java: {r.status_code} {r.text[:200]}")
    except Exception as e:
        print(f"[PY CAM] Erro login Java: {e}")
    return None


def pode_enviar(tag_id: int) -> bool:
    t = time.time()
    prev = last_sent.get(tag_id, 0)
    return (t - prev) >= ENVIO_INTERVALO


def enviar_tag(tag_id: int):
    global jwt_token
    headers = {"Content-Type": "application/json", "Accept": "application/json"}
    if jwt_token:
        headers["Authorization"] = f"Bearer {jwt_token}"

    payload = {"codigo": f"ARUCO-{tag_id}", "status": "DETECTADO", "idMoto": 1}

    try:
        r = requests.post(
            ARUCO_POST_URL, json=payload, headers=headers, timeout=API_TIMEOUT
        )
        if r.status_code in (200, 201):
            last_sent[tag_id] = time.time()
            print(f"[PY CAM] Tag {tag_id} enviada.")
        elif r.status_code == 401:
            # tentar relogar uma vez
            print("[PY CAM] 401 - tentando relogar...")
            jwt_token = login_java()
        else:
            print(f"[PY CAM] Erro ao enviar {tag_id}: {r.status_code} {r.text[:200]}")
    except Exception as e:
        print(f"[PY CAM] Erro envio tag {tag_id}: {e}")


def preprocess_gray(gray: np.ndarray) -> np.ndarray:
    """Aplica CLAHE e leve desfoque para melhorar contraste e reduzir ruído."""
    try:
        clahe = cv2.createCLAHE(clipLimit=3.0, tileGridSize=(8, 8))
        gray = clahe.apply(gray)
    except Exception:
        gray = cv2.equalizeHist(gray)
    # leve desfoque para reduzir ruído sem perder bordas
    gray = cv2.GaussianBlur(gray, (3, 3), 0)
    return gray


def process_frame(frame):
    # cv2 is a C-extension; pylint can't introspect its members reliably
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)  # pylint: disable=no-member
    gray = preprocess_gray(gray)

    # Detectar marcadores com segurança quanto ao retorno
    try:
        result = ARUCO_DETECTOR.detectMarkers(gray)
    except Exception:
        result = ([], None, [])

    corners: list = []
    ids = None
    rejected = []
    if isinstance(result, tuple):
        if len(result) >= 1 and result[0] is not None:
            corners = result[0]
        if len(result) >= 2:
            ids = result[1]
        if len(result) >= 3 and result[2] is not None:
            rejected = result[2]

    # Desenhar marcadores detectados (quando houver)
    if ids is not None and corners:
        try:
            cv2.aruco.drawDetectedMarkers(
                frame, corners, ids, borderColor=(0, 255, 0)
            )  # pylint: disable=no-member
        except Exception:
            pass
        try:
            _rvecs, _tvecs, _ = (
                cv2.aruco.estimatePoseSingleMarkers(  # pylint: disable=no-member
                    corners, REAL_MARKER_SIZE_M, CAMERA_MATRIX, DIST_COEFFS
                )
            )
        except Exception:
            _rvecs, _tvecs = None, None

        for i, marker_id in enumerate(ids or []):
            try:
                tag_id = int(marker_id[0])
            except Exception:
                continue
            # draw info (com fallback de coordenada caso corners não esteja no formato esperado)
            try:
                pt_arr = corners[i][0][0]
                pt = tuple(np.array(pt_arr).astype(int))
            except Exception:
                pt = (10, 30)
            cv2.putText(
                frame,
                f"ID: {tag_id}",
                (pt[0], pt[1] - 10),
                cv2.FONT_HERSHEY_SIMPLEX,
                0.6,
                (0, 255, 0),
                2,
            )
            # enviar para backend (com rate limit simples)
            try:
                if pode_enviar(tag_id):
                    enviar_tag(tag_id)
            except Exception:
                pass

    # overlay contador
    count = 0 if ids is None else len(ids)
    txt = f"ArUco: {count} | Dict: {CURRENT_DICT_NAME} | API: {'ON' if jwt_token else 'LOGIN?'}"
    (w, h), _ = cv2.getTextSize(txt, cv2.FONT_HERSHEY_SIMPLEX, 0.6, 2)
    cv2.rectangle(frame, (5, 5), (15 + w, 30 + h), (0, 0, 0), -1)
    cv2.putText(frame, txt, (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 0.6, (255, 255, 255), 2)
    return frame


def _apply_camera_preferences(cap) -> None:
    """Tenta aplicar resolução e expo auto desabilitado para estabilidade (ignorando falhas)."""
    try:
        cap.set(cv2.CAP_PROP_FRAME_WIDTH, 1280)
        cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 720)
        cap.set(cv2.CAP_PROP_FPS, 30)
        # Tentar desabilitar auto-exposição (valores variam por backend)
        cap.set(cv2.CAP_PROP_AUTO_EXPOSURE, 0.25)  # DSHOW/MSMF
    except Exception:
        pass


def capture_loop():
    global running, latest_frame, jwt_token
    cap = cv2.VideoCapture(selected_camera_id)
    if not cap.isOpened():
        print(f"[PY CAM] Não foi possível abrir a câmera {selected_camera_id}")
        running = False
        return

    print(f"[PY CAM] Captura iniciada na câmera {selected_camera_id}")
    _apply_camera_preferences(cap)
    # Garantir token no início
    if not jwt_token:
        jwt_token = login_java()

    frame_count = 0
    try:
        while running:
            ok, frame = cap.read()
            if not ok:
                print(
                    f"[PY CAM] AVISO: Falha ao ler frame da câmera {selected_camera_id}"
                )
                time.sleep(0.05)
                continue

            frame = process_frame(frame)
            # atualiza frame
            with frame_lock:
                latest_frame = frame.copy()

            frame_count += 1
            if frame_count % 100 == 0:  # Log a cada 100 frames
                print(
                    f"[PY CAM] Processados {frame_count} frames (câmera {selected_camera_id})"
                )

            # Pequeno delay para não sobrecarregar
            time.sleep(0.01)
    finally:
        cap.release()
        print(f"[PY CAM] Captura encerrada. Total de frames processados: {frame_count}")
        # Limpar o último frame ao parar
        with frame_lock:
            latest_frame = None


@app.route("/status")
def status():
    return jsonify(
        {
            "running": running,
            "has_frame": latest_frame is not None,
            "camera_id": selected_camera_id,
            "dict": CURRENT_DICT_NAME,
            "thread_alive": (
                capture_thread is not None and capture_thread.is_alive()
                if capture_thread
                else False
            ),
        }
    )


@app.route("/debug")
def debug():
    """Endpoint de debug para diagnóstico."""
    return jsonify(
        {
            "running": running,
            "has_frame": latest_frame is not None,
            "camera_id": selected_camera_id,
            "dict": CURRENT_DICT_NAME,
            "detector_params": {
                "adaptiveThreshWinSizeMin": int(ARUCO_PARAMS.adaptiveThreshWinSizeMin),
                "adaptiveThreshWinSizeMax": int(ARUCO_PARAMS.adaptiveThreshWinSizeMax),
                "adaptiveThreshWinSizeStep": int(
                    ARUCO_PARAMS.adaptiveThreshWinSizeStep
                ),
                "adaptiveThreshConstant": float(ARUCO_PARAMS.adaptiveThreshConstant),
                "minMarkerPerimeterRate": float(ARUCO_PARAMS.minMarkerPerimeterRate),
                "cornerRefinementMethod": int(ARUCO_PARAMS.cornerRefinementMethod),
            },
            "jwt_token_exists": jwt_token is not None,
            "capture_thread_alive": (
                capture_thread.is_alive() if capture_thread else False
            ),
            "frame_shape": latest_frame.shape if latest_frame is not None else None,
        }
    )


def _open_capture(index: int):
    """Tenta abrir o dispositivo de câmera usando os backends mais estáveis no Windows.
    Sempre retorne um objeto VideoCapture (aberto ou não) para garantir release posterior.
    """
    backends = []
    # Preferir DirectShow no Windows para evitar travas na enumeração
    if os.name == "nt":
        backends = [cv2.CAP_DSHOW, cv2.CAP_MSMF, cv2.CAP_ANY]
    else:
        backends = [cv2.CAP_ANY]

    cap = None
    for be in backends:
        try:
            cap = cv2.VideoCapture(index, be)
            if cap is not None and cap.isOpened():
                return cap
            # se não abriu, tenta próximo backend
            if cap is not None:
                cap.release()
        except Exception:
            try:
                if cap is not None:
                    cap.release()
            except Exception:
                pass
            continue
    # última tentativa genérica
    cap = cv2.VideoCapture(index)
    return cap


@app.route("/cameras")
def list_cameras():
    """Enumera câmeras disponíveis rapidamente sem capturar frames.
    Evita bloqueios de read() e sempre libera o dispositivo.
    Usa no máximo 5 índices por padrão (0..4) para rapidez; ajuste com ?max=10.
    Quando nenhuma câmera for detectada, fornece sugestões de IDs (0..2) para tentativa manual.
    """
    try:
        max_idx = int(request.args.get("max", "5"))
    except Exception:
        max_idx = 5

    max_idx = max(1, min(max_idx, 20))  # limitar entre 1 e 20

    available = []
    for i in range(max_idx):
        cap = None
        try:
            cap = _open_capture(i)
            if cap is not None and cap.isOpened():
                available.append({"id": i, "name": f"Câmera {i}", "detected": True})
        finally:
            try:
                if cap is not None:
                    cap.release()
            except Exception:
                pass

    # Fallback: sugerir IDs comuns se nada foi detectado (permite usuário tentar mesmo assim)
    if len(available) == 0:
        suggested = [
            {"id": i, "name": f"Câmera (tentativa) {i}", "detected": False}
            for i in range(3)
        ]
        return jsonify(
            {
                "cameras": suggested,
                "selected": selected_camera_id,
                "note": "Nenhuma câmera detectada; exibindo sugestões para tentativa manual.",
            }
        )

    return jsonify({"cameras": available, "selected": selected_camera_id})


@app.route("/start", methods=["POST"])
def start():
    global running, capture_thread, selected_camera_id, CURRENT_DICT_NAME, ARUCO_DETECTOR, ARUCO_PARAMS, ARUCO_DICT
    if running:
        return jsonify({"ok": True, "message": "Já está rodando"})

    # Aceita camera_id no body JSON
    data = request.get_json(silent=True) or {}
    cam_id = data.get("camera_id")
    if cam_id is not None:
        selected_camera_id = int(cam_id)

    # Aceita aruco_dict opcional (ex.: "DICT_6X6_250")
    dict_name = data.get("aruco_dict")
    if isinstance(dict_name, str) and dict_name in DICT_MAP:
        CURRENT_DICT_NAME = dict_name
        ARUCO_DETECTOR, ARUCO_PARAMS, ARUCO_DICT = build_detector(CURRENT_DICT_NAME)

    running = True
    t = threading.Thread(target=capture_loop, daemon=True)
    t.start()
    capture_thread = t
    return jsonify(
        {"ok": True, "camera_id": selected_camera_id, "dict": CURRENT_DICT_NAME}
    )


@app.route("/stop", methods=["POST"])
def stop():
    global running
    running = False
    return jsonify({"ok": True})


@app.route("/config", methods=["GET", "POST"])
def config():
    """GET: retorna config atual. POST: altera dicionário atual via {"aruco_dict": "DICT_*"}."""
    global CURRENT_DICT_NAME, ARUCO_DETECTOR, ARUCO_PARAMS, ARUCO_DICT
    if request.method == "POST":
        data = request.get_json(silent=True) or {}
        dict_name = data.get("aruco_dict")
        if isinstance(dict_name, str) and dict_name in DICT_MAP:
            CURRENT_DICT_NAME = dict_name
            ARUCO_DETECTOR, ARUCO_PARAMS, ARUCO_DICT = build_detector(CURRENT_DICT_NAME)
            return jsonify({"ok": True, "dict": CURRENT_DICT_NAME})
        return jsonify({"ok": False, "error": "Dicionário inválido"}), 400
    else:
        return jsonify(
            {
                "ok": True,
                "dict": CURRENT_DICT_NAME,
                "available_dicts": list(DICT_MAP.keys()),
            }
        )


def mjpeg_generator():
    """Gera stream MJPEG contínuo a partir dos frames capturados."""
    print("[PY CAM] Stream MJPEG iniciado")
    frame_counter = 0
    while True:
        with frame_lock:
            frame = None if latest_frame is None else latest_frame.copy()
        if frame is None:
            time.sleep(0.05)
            continue

        ret, jpeg = cv2.imencode(".jpg", frame, [int(cv2.IMWRITE_JPEG_QUALITY), 80])
        if not ret:
            continue

        buf = jpeg.tobytes()
        frame_counter += 1
        if frame_counter % 100 == 0:
            print(f"[PY CAM] Stream: {frame_counter} frames enviados")

        yield (b"--frame\r\n" b"Content-Type: image/jpeg\r\n\r\n" + buf + b"\r\n")

        # Pequeno delay para controlar taxa de frames (30 FPS)
        time.sleep(0.033)


@app.route("/stream")
def stream():
    """Stream MJPEG - sem cache para garantir vídeo ao vivo."""
    response = Response(
        mjpeg_generator(), mimetype="multipart/x-mixed-replace; boundary=frame"
    )
    # Desabilitar cache completamente
    response.headers["Cache-Control"] = "no-cache, no-store, must-revalidate"
    response.headers["Pragma"] = "no-cache"
    response.headers["Expires"] = "0"
    return response


UI_HTML = """
<!doctype html>
<html>
  <head>
    <meta charset="utf-8" />
    <title>MottuFlow - Câmera IoT</title>
    <style>
      body { font-family: Arial, sans-serif; margin: 20px; }
      .row { display: flex; gap: 12px; align-items: center; }
      button { padding: 8px 14px; }
      #status { margin-left: 8px; }
      img { border: 1px solid #ccc; width: 720px; height: 405px; object-fit: cover; }
    </style>
  </head>
  <body>
    <h1>MottuFlow - Câmera IoT</h1>
    <div class="row">
      <button onclick="startCam()">Start</button>
      <button onclick="stopCam()">Stop</button>
      <span id="status">status...</span>
    </div>
    <p></p>
    <img id="view" src="/stream" alt="stream" />

    <script>
      async function startCam() {
        await fetch('/start', {method:'POST'});
        setTimeout(refreshStatus, 500);
      }
      async function stopCam() {
        await fetch('/stop', {method:'POST'});
        setTimeout(refreshStatus, 500);
      }
      async function refreshStatus(){
        try{
          const r = await fetch('/status');
          const j = await r.json();
          document.getElementById('status').innerText = j.running ? 'Rodando' : 'Parado';
        }catch(e){ document.getElementById('status').innerText='Erro'; }
      }
      refreshStatus();
      setInterval(refreshStatus, 2000);
    </script>
  </body>
</html>
"""


@app.route("/ui")
def ui():
    return render_template_string(UI_HTML)


if __name__ == "__main__":
    print("Camera Web Service ouvindo em http://localhost:5001/ui")
    app.run(host="0.0.0.0", port=5001, debug=False)
