#!/bin/bash

# Define LocalStack endpoint and bucket name
BUCKET_NAME=cold-storage-bucket

# Create the S3 bucket using awslocal
if awslocal s3api create-bucket --bucket $BUCKET_NAME; then
    # Print a success message if the command succeeded
    echo "S3 bucket '$BUCKET_NAME' created successfully."
fi