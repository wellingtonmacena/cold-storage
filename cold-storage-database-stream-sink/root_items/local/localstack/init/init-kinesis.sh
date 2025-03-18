#!/bin/bash
set -e

echo "Criando stream no Kinesis..."
if awslocal kinesis create-stream --stream-name cold-storage-stream --shard-count 1; then
    echo "Stream 'cold-storage-stream' criado com sucesso!"
else
    echo "Falha ao criar o stream 'cold-storage-stream'."
fi
