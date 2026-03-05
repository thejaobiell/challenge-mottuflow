"""
Teste de integracao: Python -> Java API -> Frontend
- Faz login para obter JWT
- Envia uma ArUco Tag fake para a API Java (com Authorization)
- Lista as tags e imprime a quantidade
Requisitos: somente 'requests' (sem OpenCV)
"""

import requests

BASE = "http://localhost:8080"
LOGIN_URL = f"{BASE}/api/login"
API_POST_URL = f"{BASE}/api/aruco-tags/cadastrar"
API_GET_URL = f"{BASE}/api/aruco-tags/listar"

payload = {
    "codigo": "ARUCO-9999",
    "status": "DETECTADO",
    "idMoto": 1,
}


def try_json(resp):
    try:
        return resp.json()
    except Exception:
        return None


try:
    print("[PY] Fazendo login (JWT)...")
    login_body = {"email": "admin@email.com", "senha": "adminmottu"}
    lr = requests.post(LOGIN_URL, json=login_body, timeout=15)
    print(f"[PY] LOGIN status: {lr.status_code}")
    token = None
    if lr.ok:
        data = try_json(lr)
        if data and "tokenAcesso" in data:
            token = data["tokenAcesso"]
            print("[PY] Login OK, token obtido (primeiros 20):", token[:20], "...")
        else:
            print("[PY] Login respondeu sem token. Corpo:", lr.text[:200])
    else:
        print("[PY] Falha no login. Corpo:", lr.text[:200])

    headers = {"Authorization": f"Bearer {token}"} if token else {}

    print("[PY] Enviando tag de teste para a API Java...")
    r = requests.post(API_POST_URL, json=payload, headers=headers, timeout=15)
    print(f"[PY] POST status: {r.status_code}")
    if not r.ok:
        print("[PY] Resposta POST:", r.text[:300])
    else:
        print("[PY] POST OK", try_json(r) or r.text[:200])

    print("[PY] Buscando lista de tags...")
    r2 = requests.get(API_GET_URL, headers=headers, timeout=15)
    print(f"[PY] GET status: {r2.status_code}")
    if r2.ok:
        data = try_json(r2)
        if data is not None:
            print(f"[PY] Total de tags retornadas: {len(data)}")
            if data:
                print("[PY] Exemplo da primeira tag:", data[0])
        else:
            print("[PY] GET OK mas nao retornou JSON. Corpo:", r2.text[:300])
    else:
        print("[PY] GET falhou. Corpo:", r2.text[:300])

    print("[PY] Teste de integracao Python->Java concluido.")
except Exception as e:
    print("[PY] ERRO no teste de integracao:", e)
