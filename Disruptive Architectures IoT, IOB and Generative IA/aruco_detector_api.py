"""
MottuFlow - Detector de ArUco Tags com Integração API
Este script detecta códigos QR ArUco e envia os dados para o backend Java
"""

import cv2
import numpy as np
import requests
import json
import time
from datetime import datetime

# ========== CONFIGURAÇÕES ==========
# API Backend Java
API_BASE_URL = "http://localhost:8080/api/aruco-tags/cadastrar"
API_GET_URL = "http://localhost:8080/api/aruco-tags/listar"
API_TIMEOUT = 5  # segundos

# Configurações de Câmera
CAMERA_ID = 0  # 0 para webcam padrão
CAMERA_NAME = "Camera Principal"

# Configurações ArUco
ARUCO_DICT = cv2.aruco.getPredefinedDictionary(cv2.aruco.DICT_6X6_250)
ARUCO_PARAMS = cv2.aruco.DetectorParameters()
ARUCO_DETECTOR = cv2.aruco.ArucoDetector(ARUCO_DICT, ARUCO_PARAMS)

# Parâmetros de câmera para estimativa de distância
REAL_MARKER_SIZE_M = 0.05  # Tamanho real do marcador em metros
CAMERA_MATRIX = np.array([[1000, 0, 640], [0, 1000, 360], [0, 0, 1]], dtype=np.float32)
DIST_COEFFS = np.zeros((4, 1), dtype=np.float32)

# Cores
COLOR_ARUCO = (0, 255, 0)  # Verde
COLOR_TEXT = (255, 255, 255)  # Branco
COLOR_BG = (0, 0, 0)  # Preto

# Controle de envio
ENVIO_INTERVALO = 2  # segundos entre envios da mesma tag
tags_enviadas = {}  # {tag_id: timestamp_ultimo_envio}


class APIClient:
    """Cliente para comunicação com a API Java"""

    @staticmethod
    def enviar_aruco_tag(tag_id, id_moto=None):
        """
        Envia informações da tag ArUco para a API Java

        Args:
            tag_id: ID do marcador ArUco detectado
            id_moto: ID da moto associada (opcional)

        Returns:
            dict: Resposta da API ou None em caso de erro
        """
        try:
            # Dados a serem enviados (seguindo o DTO do Java)
            payload = {
                "codigo": f"ARUCO-{tag_id}",
                "status": "DETECTADO",
                "idMoto": id_moto if id_moto else 1,  # Moto padrão se não especificado
            }

            # Headers
            headers = {"Content-Type": "application/json", "Accept": "application/json"}

            # Faz a requisição POST
            response = requests.post(
                API_BASE_URL, json=payload, headers=headers, timeout=API_TIMEOUT
            )

            if response.status_code == 201 or response.status_code == 200:
                print(f"✅ Tag {tag_id} enviada com sucesso!")
                return response.json()
            else:
                print(f"⚠️ Erro ao enviar tag {tag_id}: Status {response.status_code}")
                print(f"Resposta: {response.text}")
                return None

        except requests.exceptions.Timeout:
            print(f"⏰ Timeout ao enviar tag {tag_id}")
            return None
        except requests.exceptions.ConnectionError:
            print(f"🔌 Erro de conexão com a API. Verifique se o backend está rodando.")
            return None
        except Exception as e:
            print(f"❌ Erro inesperado ao enviar tag {tag_id}: {str(e)}")
            return None

    @staticmethod
    def buscar_tags():
        """
        Busca todas as tags cadastradas na API

        Returns:
            list: Lista de tags ou None em caso de erro
        """
        try:
            response = requests.get(API_GET_URL, timeout=API_TIMEOUT)
            if response.status_code == 200:
                return response.json()
            return None
        except Exception as e:
            print(f"❌ Erro ao buscar tags: {str(e)}")
            return None


def pode_enviar_tag(tag_id):
    """
    Verifica se a tag pode ser enviada novamente
    (evita spam de envios da mesma tag)
    """
    agora = time.time()
    if tag_id not in tags_enviadas:
        return True

    tempo_decorrido = agora - tags_enviadas[tag_id]
    return tempo_decorrido >= ENVIO_INTERVALO


def processar_frame(frame, enviar_para_api=True):
    """
    Processa o frame detectando marcadores ArUco

    Args:
        frame: Frame capturado da câmera
        enviar_para_api: Se True, envia detecções para a API

    Returns:
        frame: Frame processado com anotações
    """
    aruco_count = 0

    # Converte para escala de cinza
    gray = cv2.cvtColor(frame, cv2.COLOR_BGR2GRAY)

    # Detecta marcadores ArUco
    corners, ids, rejected = ARUCO_DETECTOR.detectMarkers(gray)

    if ids is not None:
        aruco_count = len(ids)

        # Desenha marcadores detectados
        cv2.aruco.drawDetectedMarkers(frame, corners, ids, borderColor=COLOR_ARUCO)

        # Estima pose e distância
        rvecs, tvecs, _ = cv2.aruco.estimatePoseSingleMarkers(
            corners, REAL_MARKER_SIZE_M, CAMERA_MATRIX, DIST_COEFFS
        )

        for i, marker_id in enumerate(ids):
            tag_id = int(marker_id[0])
            distancia = tvecs[i][0][2]

            # Posição para o texto
            canto_superior_esquerdo = tuple(corners[i][0][0].astype(int))

            # Informações do marcador
            id_texto = f"ID: {tag_id}"
            dist_texto = f"Dist: {distancia:.2f}m"

            # Desenha informações
            cv2.putText(
                frame,
                id_texto,
                (canto_superior_esquerdo[0], canto_superior_esquerdo[1] - 30),
                cv2.FONT_HERSHEY_SIMPLEX,
                0.6,
                COLOR_ARUCO,
                2,
            )
            cv2.putText(
                frame,
                dist_texto,
                (canto_superior_esquerdo[0], canto_superior_esquerdo[1] - 10),
                cv2.FONT_HERSHEY_SIMPLEX,
                0.6,
                COLOR_ARUCO,
                2,
            )

            # Envia para API se habilitado e se passou o intervalo
            if enviar_para_api and pode_enviar_tag(tag_id):
                resultado = APIClient.enviar_aruco_tag(tag_id)
                if resultado:
                    tags_enviadas[tag_id] = time.time()
                    # Feedback visual
                    cv2.putText(
                        frame,
                        "ENVIADO!",
                        (canto_superior_esquerdo[0], canto_superior_esquerdo[1] - 50),
                        cv2.FONT_HERSHEY_SIMPLEX,
                        0.5,
                        (0, 255, 255),
                        2,
                    )

    # Contador na tela
    contador_texto = f"ArUco Tags: {aruco_count}"
    (w, h), _ = cv2.getTextSize(contador_texto, cv2.FONT_HERSHEY_SIMPLEX, 0.7, 2)
    cv2.rectangle(frame, (5, 5), (15 + w, 35 + h), COLOR_BG, -1)
    cv2.putText(
        frame, contador_texto, (10, 30), cv2.FONT_HERSHEY_SIMPLEX, 0.7, COLOR_TEXT, 2
    )

    # Status da API
    status_texto = "API: Conectado" if enviar_para_api else "API: Desconectado"
    status_cor = (0, 255, 0) if enviar_para_api else (0, 0, 255)
    cv2.putText(
        frame, status_texto, (10, 60), cv2.FONT_HERSHEY_SIMPLEX, 0.6, status_cor, 2
    )

    return frame


def run_detector(enviar_para_api=True):
    """
    Inicia o detector de ArUco em tempo real

    Args:
        enviar_para_api: Se True, envia detecções para a API Java
    """
    print("=" * 60)
    print("🚀 MottuFlow - Detector ArUco IoT")
    print("=" * 60)
    print(f"📹 Câmera: {CAMERA_NAME}")
    print(f"🔗 API: {API_BASE_URL}")
    print(f"📊 Enviando para API: {'Sim' if enviar_para_api else 'Não'}")
    print("=" * 60)
    print("\n⌨️  Controles:")
    print("  [Q] - Sair")
    print("  [S] - Alternar envio para API")
    print("  [L] - Listar tags cadastradas")
    print("=" * 60)

    # Abre a câmera
    cap = cv2.VideoCapture(CAMERA_ID)

    if not cap.isOpened():
        print(f"❌ Erro: Não foi possível abrir a câmera {CAMERA_ID}")
        return

    print("\n✅ Câmera iniciada. Aguardando detecções...")

    enviar_api_ativo = enviar_para_api

    try:
        while True:
            ret, frame = cap.read()

            if not ret:
                print("❌ Erro ao capturar frame")
                break

            # Processa o frame
            processed_frame = processar_frame(frame, enviar_api_ativo)

            # Mostra o resultado
            cv2.imshow("MottuFlow - ArUco Detector", processed_frame)

            # Controles de teclado
            key = cv2.waitKey(1) & 0xFF

            if key == ord("q") or key == ord("Q"):
                print("\n🛑 Encerrando detector...")
                break
            elif key == ord("s") or key == ord("S"):
                enviar_api_ativo = not enviar_api_ativo
                status = "ATIVADO" if enviar_api_ativo else "DESATIVADO"
                print(f"\n🔄 Envio para API: {status}")
            elif key == ord("l") or key == ord("L"):
                print("\n📋 Buscando tags cadastradas...")
                tags = APIClient.buscar_tags()
                if tags:
                    print(f"Total de tags: {len(tags)}")
                    for tag in tags[:10]:  # Mostra apenas as 10 primeiras
                        print(
                            f"  - ID: {tag.get('id')}, Código: {tag.get('codigo')}, Status: {tag.get('status')}"
                        )

    except KeyboardInterrupt:
        print("\n⚠️ Interrompido pelo usuário")

    finally:
        cap.release()
        cv2.destroyAllWindows()
        print("✅ Detector encerrado")


if __name__ == "__main__":
    # Inicia o detector com envio para API habilitado
    run_detector(enviar_para_api=True)
