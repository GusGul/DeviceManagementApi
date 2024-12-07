FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG APP_UID=1000
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY *.csproj .
RUN dotnet restore
COPY . .

WORKDIR "/src/DeviceManagementApiPresentation"
RUN dotnet build "DeviceManagementApiPresentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "DeviceManagementApiPresentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeviceManagementApiPresentation.dll"]
