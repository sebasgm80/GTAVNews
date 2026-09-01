#!/bin/bash
XBOX_IP="${XBOX_IP:-192.168.1.129}"
USERNAME="${XBOX_USER:-tu_usuario}"
PASSWORD="${XBOX_PASS:-tu_contraseña}"
MSIX_FILE="${1:-GTAVNews.msix}"

if [ ! -f "$MSIX_FILE" ]; then
  echo "Error: no encuentro ${MSIX_FILE}"
  echo "Uso: XBOX_USER=usuario XBOX_PASS=pass ./deploy.sh archivo.msix"
  exit 1
fi

echo "Desplegando en Xbox..."
curl -X POST "http://${XBOX_IP}:11443/api/uploaddata?file=$(basename "$MSIX_FILE")&overwrite=true" --data-binary "@${MSIX_FILE}" -u "${USERNAME}:${PASSWORD}" -v
curl -X POST "http://${XBOX_IP}:11443/api/installs?type=install&file=$(basename "$MSIX_FILE")" -u "${USERNAME}:${PASSWORD}" -v
echo "Hecho!"