"""
Script de teste para verificar se o stream da câmera está transmitindo frames diferentes.
Captura 5 frames consecutivos e compara para garantir que não são idênticos.
"""

import requests
import time
import hashlib

STREAM_URL = "http://localhost:5001/stream"
NUM_FRAMES = 5

print("=" * 60)
print("TESTE DE STREAM - Verificando se frames estão mudando")
print("=" * 60)
print(f"\nURL do stream: {STREAM_URL}")
print(f"Capturando {NUM_FRAMES} frames...\n")

try:
    # Fazer request ao stream
    response = requests.get(STREAM_URL, stream=True, timeout=10)

    if response.status_code != 200:
        print(f"ERRO: Status {response.status_code}")
        exit(1)

    frames_hashes = []
    frame_count = 0
    buffer = b""

    # Ler stream MJPEG
    for chunk in response.iter_content(chunk_size=1024):
        buffer += chunk

        # Procurar por delimitador de frame
        while b"\xff\xd9" in buffer:  # JPEG end marker
            # Extrair um frame completo
            end_idx = buffer.index(b"\xff\xd9") + 2
            frame_data = buffer[:end_idx]
            buffer = buffer[end_idx:]

            # Calcular hash do frame
            frame_hash = hashlib.md5(frame_data).hexdigest()
            frames_hashes.append(frame_hash)
            frame_count += 1

            print(
                f"Frame {frame_count}: hash={frame_hash[:16]}... tamanho={len(frame_data)} bytes"
            )

            if frame_count >= NUM_FRAMES:
                break

        if frame_count >= NUM_FRAMES:
            break

    print("\n" + "=" * 60)
    print("RESULTADO:")
    print("=" * 60)

    # Verificar se os frames são diferentes
    unique_frames = len(set(frames_hashes))

    if unique_frames == 1:
        print("❌ PROBLEMA: Todos os frames são IDÊNTICOS!")
        print("   A câmera está enviando sempre a mesma imagem (frame congelado)")
        print("   Verifique se o loop de captura está rodando corretamente")
    elif unique_frames == NUM_FRAMES:
        print(f"✅ OK: Todos os {NUM_FRAMES} frames são DIFERENTES!")
        print("   O stream está transmitindo vídeo ao vivo corretamente")
    else:
        print(f"⚠️  PARCIAL: {unique_frames}/{NUM_FRAMES} frames únicos")
        print("   Alguns frames se repetem (pode ser normal se a cena não muda)")

    print("\nHashes dos frames:")
    for i, h in enumerate(frames_hashes, 1):
        print(f"  Frame {i}: {h}")

except requests.exceptions.Timeout:
    print("❌ ERRO: Timeout ao conectar ao stream")
    print("   Verifique se camera_web_service.py está rodando")
except requests.exceptions.ConnectionError:
    print("❌ ERRO: Não foi possível conectar ao stream")
    print("   Certifique-se que camera_web_service.py está rodando na porta 5001")
except Exception as e:
    print(f"❌ ERRO: {e}")

print("\n" + "=" * 60)
