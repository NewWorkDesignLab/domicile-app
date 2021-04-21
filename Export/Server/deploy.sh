#!/usr/bin/env sh
set -e

# useage: /bin/bash deploy.sh export-folder-name

HOST="root@188.68.56.140"
DEPLOY_TO="/var/www/domicile/server"
RELEASE="domicile_mirror_release_${1}"
RELEASE_PATH="${DEPLOY_TO}/${RELEASE}"

echo "Clear Old Data"
cd ./Data/${1}
rm -rf server.zip
rm -rf domicile_mirror_server.tar
rm -rf Dockerfile
rm -rf cert.json

echo "Zip Server for Docker"
zip -r server.zip ./

echo "Building Docker Image"
cp ../../Dockerfile .
cp ../../cert.json .
docker build . -t domicile_mirror_server

echo "Compress Docker Image"
docker save domicile_mirror_server | gzip > domicile_mirror_server.tar

echo "Preparing SSH Deploy Directory"
ssh $HOST mkdir -p $RELEASE_PATH

echo "Copy App Image"
scp ./domicile_mirror_server.tar $HOST:$RELEASE_PATH

echo "Copy Docker-Compose Files"
scp ../../docker-compose.yml $HOST:$RELEASE_PATH

echo "Clearing Old Stack"
ssh $HOST "docker stop domicile_mirror_server_1" || true
ssh $HOST "docker rm domicile_mirror_server_1" || true

echo "Load Docker Image"
ssh $HOST "docker image load -q -i ${RELEASE_PATH}/domicile_mirror_server.tar"

echo "Starting Docker"
ssh $HOST "docker-compose -f ${RELEASE_PATH}/docker-compose.yml -p domicile_mirror up -d -V"

echo "Clear Data"
rm -rf server.zip
rm -rf domicile_mirror_server.tar
rm -rf Dockerfile
rm -rf cert.json
