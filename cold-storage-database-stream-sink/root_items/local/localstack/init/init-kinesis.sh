#!/bin/bash
set -e


echo "Criando stream no Kinesis..."
awslocal kinesis create-stream --stream-name cold-storage-stream --shard-count 1
echo "Stream 'cold-storage-stream' criado com sucesso!"
