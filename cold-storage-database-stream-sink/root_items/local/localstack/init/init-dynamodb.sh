#!/bin/bash

# Define variables
TABLE_NAME="cold-storage-data-catalog"
REGION="us-east-1"

# Define LocalStack endpoint and table name
LOCALSTACK_ENDPOINT="http://localhost:4566"

# Create the DynamoDB table using AWS CLI
if awslocal dynamodb create-table \
    --table-name "$TABLE_NAME" \
    --attribute-definitions '[{"AttributeName":"Id","AttributeType":"S"}]' \
    --key-schema '[{"AttributeName":"Id","KeyType":"HASH"}]' \
    --provisioned-throughput '{"ReadCapacityUnits":1,"WriteCapacityUnits":1}' \
    --endpoint-url "$LOCALSTACK_ENDPOINT"; then
    echo "DynamoDB table '$TABLE_NAME' created successfully."
else
    echo "Failed to create DynamoDB table '$TABLE_NAME'."
    exit 1
fi

# Verify the table was created
if awslocal dynamodb list-tables --endpoint-url "$LOCALSTACK_ENDPOINT" | grep -q "$TABLE_NAME"; then
    echo "DynamoDB table '$TABLE_NAME' exists."
else
    echo "DynamoDB table '$TABLE_NAME' does not exist."
    exit 1
fi
