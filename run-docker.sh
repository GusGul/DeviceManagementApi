#!/bin/bash

echo "Building the Docker image..."
docker build -t device-management-api .

echo "Running the Docker container..."
docker run -d -p 5000:5000 -p 5001:5001 --name device-management-api-container device-management-api

echo "API is now running at: http://localhost:5000"
