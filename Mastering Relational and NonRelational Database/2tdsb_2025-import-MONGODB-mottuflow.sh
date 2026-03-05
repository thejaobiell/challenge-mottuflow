#!/bin/bash

DB_NAME="mottuflow"

JSON_DIR="."

mongoimport --db $DB_NAME --collection funcionarios --file $JSON_DIR/funcionario.json --jsonArray
mongoimport --db $DB_NAME --collection patio --file $JSON_DIR/patio.json --jsonArray
mongoimport --db $DB_NAME --collection motos --file $JSON_DIR/moto.json --jsonArray
mongoimport --db $DB_NAME --collection camera --file $JSON_DIR/camera.json --jsonArray
mongoimport --db $DB_NAME --collection aruco_tag --file $JSON_DIR/aruco_tag.json --jsonArray
mongoimport --db $DB_NAME --collection localidade --file $JSON_DIR/localidade.json --jsonArray
mongoimport --db $DB_NAME --collection registro_status --file $JSON_DIR/registro_status.json --jsonArray
mongoimport --db $DB_NAME --collection auditoria --file $JSON_DIR/auditoria.json --jsonArray
mongoimport --db $DB_NAME --collection fato_motos_status --file $JSON_DIR/fato_motos_status.json --jsonArray

echo "Importação concluída!"
