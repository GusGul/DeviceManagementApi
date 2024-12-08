# Device Management API
This project is a Device Management API built with ASP.NET Core. It provides endpoints to manage devices, including adding, updating, and retrieving device information.

## Prerequisites
- .NET 8.0 SDK or later
- PostgreSQL database
- Visual Studio or JetBrains Rider
- Docker (optional)

## Technologies Used
- C#
- .NET
- AutoMapper
- FluentValidation
- Moq
- Npgsql
- Serilog
- xunit

## Getting Started
### Installation
1. Clone the repository:
    ```bash
    git clone https://github.com/GusGul/DeviceManagementApi.git
    cd DeviceManagementApi
    ```
2. Build the project:
    ```bash
    dotnet build
    ```

### Running the Application
To run the application locally:
Manually:
```bash
dotnet run --project ./DeviceManagementApiPresentation/DeviceManagementApiPresentation.csproj
```
Build and run script:
```bash
chmod +x build-and-run.sh
./build-and-run.sh
```
Swagger is configured for API documentation. When running the application, you can access the Swagger UI at https://localhost:5001/swagger or http://localhost:5000/swagger.

To run the application using Docker:
Running the run-docker script:
```bash
chmod +x run-docker.sh
./run-docker.sh
```
Manually:
```bash
docker build -t devicemanagementapi .
docker run -p 8080:80 devicemanagementapi
```

## Usage
### API Endpoints
| HTTP Method | Endpoint                          | Description                                       | Request Body                             | Response                                                                                     |
|-------------|-----------------------------------|---------------------------------------------------|------------------------------------------|----------------------------------------------------------------------------------------------|
| `GET`       | `/api/devices`                    | Get all devices                                  | N/A                                      | `200 OK` - List of devices or `400 Bad Request` if invalid data.                                                                  |
| `GET`       | `/api/devices/{id}`               | Get a device by ID                               | N/A                                      | `200 OK` - Device details or `404 Not Found` if device not found or `400 Bad Request` if invalid data.                         |
| `POST`      | `/api/devices`                    | Create a new device                              | `{ "name": "Device Name", "brand": "Brand"}` | `201 Created` - Device created or `400 Bad Request` if invalid data.                                                          |
| `PUT`       | `/api/devices/{id}`               | Update a device by ID                            | `{ "name": "Updated Name", "brand": "Updated Brand"}` | `200 OK` - Device updated or `404 Not Found` if device not found or `400 Bad Request` if invalid data.                           |
| `DELETE`    | `/api/devices/{id}`               | Delete a device by ID                            | N/A                                      | `204 No Content` - Device deleted or `404 Not Found` if device not found or `400 Bad Request` if invalid data.                   |
| `PATCH`     | `/api/devices/{id}`               | Partially update a device by ID                  | `{ "name": "Partial Name"}`               | `200 OK` - Device updated or `404 Not Found` if device not found or `400 Bad Request` if invalid data.                           |
| `GET`      | `/api/devices/search`        | Search devices by brand name                        | `{ "brand": "brand name" }`                        | `200 OK` - List of devices found or `400 Bad Request` if invalid data.                             |

## Contributing
1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -m 'verb: add new feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Open a Pull Request.

## Logging
The application uses Serilog for logging. Logs are written to the console and to files in the logs directory.  

## License
This project is licensed under the MIT License.
