#!/bin/bash

PROJECT_PATH="."
dotnet user-secrets list --project "$PROJECT_PATH" | while IFS=': ' read -r key value; do

# Generate .env file from user-secrets
env_key=$(echo "$key" | sed 's/:/__/g' | tr '[:lower:]' '[:upper:]')
env_value=$(echo "$value" | tr '[:lower:]' '[:upper:]')
echo "${env_key}_${env_value}"
done > ../.env

echo "✅ .env file created from user-secrets:"
cat ../.env
