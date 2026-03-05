"""
Gerador de QR Codes ArUco para o projeto MottuFlow
Este script gera marcadores ArUco que podem ser detectados pelo sistema
"""

import cv2
import numpy as np
import os
import qrcode
from PIL import Image, ImageDraw, ImageFont

# Configurações
OUTPUT_DIR = "aruco_markers"
ARUCO_DICT = cv2.aruco.DICT_6X6_250
MARKER_SIZE = 400  # pixels
START_ID = 1
END_ID = 20  # Gera 20 marcadores

# Criar diretório de saída
os.makedirs(OUTPUT_DIR, exist_ok=True)


def gerar_aruco_marker(marker_id, size=MARKER_SIZE):
    """
    Gera um marcador ArUco tradicional

    Args:
        marker_id: ID do marcador
        size: Tamanho do marcador em pixels

    Returns:
        numpy.array: Imagem do marcador
    """
    aruco_dict = cv2.aruco.getPredefinedDictionary(ARUCO_DICT)
    marker_image = cv2.aruco.generateImageMarker(aruco_dict, marker_id, size)
    return marker_image


def gerar_qr_code(codigo, size=MARKER_SIZE):
    """
    Gera um QR Code com o código ArUco

    Args:
        codigo: Código a ser inserido no QR Code (ex: ARUCO-123)
        size: Tamanho do QR Code

    Returns:
        PIL.Image: Imagem do QR Code
    """
    qr = qrcode.QRCode(
        version=1,
        error_correction=qrcode.constants.ERROR_CORRECT_H,
        box_size=10,
        border=4,
    )
    qr.add_data(codigo)
    qr.make(fit=True)

    img = qr.make_image(fill_color="black", back_color="white")
    img = img.resize((size, size), Image.Resampling.LANCZOS)

    return img


def adicionar_label(imagem, texto, font_size=30):
    """
    Adiciona um label de texto abaixo da imagem

    Args:
        imagem: Imagem PIL
        texto: Texto a ser adicionado
        font_size: Tamanho da fonte

    Returns:
        PIL.Image: Imagem com label
    """
    # Cria nova imagem com espaço para o label
    label_height = 80
    new_height = imagem.height + label_height
    new_img = Image.new("RGB", (imagem.width, new_height), "white")

    # Cola a imagem original
    new_img.paste(imagem, (0, 0))

    # Adiciona texto
    draw = ImageDraw.Draw(new_img)
    try:
        font = ImageFont.truetype("arial.ttf", font_size)
    except:
        font = ImageFont.load_default()

    # Centraliza o texto
    bbox = draw.textbbox((0, 0), texto, font=font)
    text_width = bbox[2] - bbox[0]
    text_height = bbox[3] - bbox[1]
    x = (imagem.width - text_width) // 2
    y = imagem.height + (label_height - text_height) // 2

    draw.text((x, y), texto, fill="black", font=font)

    return new_img


def gerar_todos_markers():
    """
    Gera todos os marcadores ArUco e QR Codes
    """
    print("=" * 60)
    print("🎯 Gerador de Marcadores ArUco - MottuFlow")
    print("=" * 60)
    print(f"📁 Diretório de saída: {OUTPUT_DIR}")
    print(f"🔢 Gerando marcadores de {START_ID} até {END_ID}")
    print("=" * 60)

    for marker_id in range(START_ID, END_ID + 1):
        codigo = f"ARUCO-{marker_id}"

        # 1. Gerar marcador ArUco tradicional
        print(f"\n📌 Gerando marcador {marker_id}...")
        aruco_marker = gerar_aruco_marker(marker_id)

        # Converte para PIL Image
        aruco_pil = Image.fromarray(aruco_marker)

        # Adiciona label
        aruco_com_label = adicionar_label(aruco_pil, codigo)

        # Salva marcador ArUco
        aruco_filename = os.path.join(OUTPUT_DIR, f"aruco_marker_{marker_id}.png")
        aruco_com_label.save(aruco_filename)
        print(f"  ✅ ArUco salvo: {aruco_filename}")

        # 2. Gerar QR Code
        qr_image = gerar_qr_code(codigo)
        qr_com_label = adicionar_label(qr_image, codigo)

        # Salva QR Code
        qr_filename = os.path.join(OUTPUT_DIR, f"qr_code_{marker_id}.png")
        qr_com_label.save(qr_filename)
        print(f"  ✅ QR Code salvo: {qr_filename}")

    # Gerar página de impressão com todos os marcadores
    gerar_pagina_impressao()

    print("\n" + "=" * 60)
    print("✅ Geração concluída!")
    print(f"📂 Arquivos salvos em: {OUTPUT_DIR}/")
    print("=" * 60)


def gerar_pagina_impressao(markers_por_linha=4, tipo="aruco"):
    """
    Gera uma página A4 com múltiplos marcadores para impressão

    Args:
        markers_por_linha: Quantidade de marcadores por linha
        tipo: 'aruco' ou 'qr'
    """
    print(f"\n📄 Gerando página de impressão ({tipo})...")

    # Configurações de página A4 (300 DPI)
    A4_WIDTH = 2480  # pixels
    A4_HEIGHT = 3508  # pixels
    MARGIN = 100
    SPACING = 50

    # Calcula tamanho dos marcadores
    markers_count = END_ID - START_ID + 1
    markers_por_coluna = (markers_count + markers_por_linha - 1) // markers_por_linha

    available_width = A4_WIDTH - 2 * MARGIN - (markers_por_linha - 1) * SPACING
    available_height = A4_HEIGHT - 2 * MARGIN - (markers_por_coluna - 1) * SPACING

    marker_size = (
        min(
            available_width // markers_por_linha, available_height // markers_por_coluna
        )
        - 100
    )  # 100 pixels para label

    # Cria página em branco
    page = Image.new("RGB", (A4_WIDTH, A4_HEIGHT), "white")

    # Adiciona título
    draw = ImageDraw.Draw(page)
    try:
        title_font = ImageFont.truetype("arial.ttf", 60)
        marker_font = ImageFont.truetype("arial.ttf", 30)
    except:
        title_font = ImageFont.load_default()
        marker_font = ImageFont.load_default()

    title = f"MottuFlow - Marcadores ArUco ({START_ID} a {END_ID})"
    bbox = draw.textbbox((0, 0), title, font=title_font)
    title_width = bbox[2] - bbox[0]
    draw.text(((A4_WIDTH - title_width) // 2, 50), title, fill="black", font=title_font)

    # Adiciona marcadores
    x = MARGIN
    y = MARGIN + 150
    count = 0

    for marker_id in range(START_ID, END_ID + 1):
        # Carrega marcador
        if tipo == "aruco":
            marker_file = os.path.join(OUTPUT_DIR, f"aruco_marker_{marker_id}.png")
        else:
            marker_file = os.path.join(OUTPUT_DIR, f"qr_code_{marker_id}.png")

        try:
            marker_img = Image.open(marker_file)
            marker_img = marker_img.resize(
                (marker_size, marker_size + 80), Image.Resampling.LANCZOS
            )

            # Cola na página
            page.paste(marker_img, (x, y))

            # Próxima posição
            x += marker_size + SPACING
            count += 1

            if count % markers_por_linha == 0:
                x = MARGIN
                y += marker_size + 80 + SPACING
        except Exception as e:
            print(f"  ⚠️ Erro ao adicionar marcador {marker_id}: {e}")

    # Salva página
    page_filename = os.path.join(OUTPUT_DIR, f"impressao_{tipo}_todos.png")
    page.save(page_filename, dpi=(300, 300))
    print(f"  ✅ Página salva: {page_filename}")


if __name__ == "__main__":
    try:
        gerar_todos_markers()

        print("\n💡 Dicas:")
        print("  • Imprima os marcadores em papel branco de boa qualidade")
        print("  • Mantenha os marcadores planos e sem dobras")
        print("  • Use iluminação adequada para melhor detecção")
        print("  • O tamanho real do marcador afeta a distância de detecção")
        print("  • QR Codes podem ser escaneados com smartphones")
        print("  • Marcadores ArUco são para detecção com câmeras OpenCV")

    except Exception as e:
        print(f"\n❌ Erro: {e}")
        import traceback

        traceback.print_exc()
