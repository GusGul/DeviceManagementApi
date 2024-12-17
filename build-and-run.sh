#!/bin/bash

PROJECT_PATH="./DeviceManagementApiPresentation/DeviceManagementApiPresentation.csproj"

OUTPUT_PATH="./bin/Release/net6.0/"

if [ -d "$OUTPUT_PATH" ]; then
    echo "Cleaning the output directory..."
    rm -rf "$OUTPUT_PATH"
fi

echo "Restoring project dependencies..."
dotnet restore $PROJECT_PATH

echo "Compiling project..."
dotnet build $PROJECT_PATH -c Release

echo "Running the app..."
dotnet run --project $PROJECT_PATH
