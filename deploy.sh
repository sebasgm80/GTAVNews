#!/bin/bash
XBOX_IP="192.168.1.129"
USERNAME="tu_usuario"
PASSWORD="tu_contraseña"
MSIX_FILE="${1:-GTAVNews.msix}"

if [ ! -f "$MSIX_FILE" ]; then
  echo "Error: No encuentro ${MSIX_FILE}. Pon el archivo .msix en esta carpeta."
  echo "Uso: ./deploy.sh ruta/al/archivo.msix"
  exit 1
fi

echo "Desplegando $(basename $MSIX_FILE) en Xbox..."
curl -X POST "http://${XBOX_IP}:11443/api/uploaddata?file=$(basename "$MSIX_FILE")&overwrite=true" --data-binary "@${MSIX_FILE}" -u "${USERNAME}:${PASSWORD}" -v
curl -X POST "http://${XBOX_IP}:11443/api/installs?type=install&file=$(basename "$MSIX_FILE")" -u "${USERNAME}:${PASSWORD}" -v
echo "Hecho!"