#!/bin/bash

# Define o caminho para o diretório do projeto
PROJECT_PATH="./DeviceManagementApiPresentation/DeviceManagementApiPresentation.csproj"

# Caminho onde a aplicação será executada
OUTPUT_PATH="./bin/Release/net6.0/"

# Verifica se o diretório de saída já existe, e se sim, limpa
if [ -d "$OUTPUT_PATH" ]; then
    echo "Limpeza do diretório de saída..."
    rm -rf "$OUTPUT_PATH"
fi

# Restaura dependências do projeto
echo "Restaurando dependências do projeto..."
dotnet restore $PROJECT_PATH

# Compila o projeto
echo "Compilando o projeto..."
dotnet build $PROJECT_PATH -c Release

# Executa a aplicação
echo "Executando a aplicação..."
dotnet run --project $PROJECT_PATH
