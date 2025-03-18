#!/bin/bash


STREAM_NAME="cold-storage-stream"  # Nome do seu Kinesis Stream
REGION="us-east-1"  # Região do seu Kinesis
ACCESS_KEY="test"                  # Chave de acesso AWS
SECRET_KEY="test"                  # Chave secreta AWS
# Loop para gerar 20 objetos e enviar para o Kinesis
for i in {1..20}
do
  # Gerar um UUID
  UUID=$(uuidgen)

  # Criar o objeto JSON
  JSON_OBJECT="{\"Key\":\"$UUID\", \"Value\":\"DATA $UUID $UUID\"}"
# Converter o JSON_OBJECT em uma sequência de bytes
    BYTE_ARRAY=$(echo -n "$JSON_OBJECT" | base64)

  # Enviar para o Kinesis Stream
  aws kinesis put-record --stream-name "$STREAM_NAME" --partition-key "$UUID" --data "$BYTE_ARRAY" --endpoint-url http://localhost:4566

done
