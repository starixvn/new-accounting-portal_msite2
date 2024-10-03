FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder
WORKDIR /src

COPY ./src/Core/*.csproj ./src/Core/
COPY ./src/Maintenance.Sync/*.csproj ./src/Maintenance.Sync/
COPY ./src/Maintenance.SpecialLeagueSetting/*.csproj ./src/Maintenance.SpecialLeagueSetting/
COPY ./src/Maintenance.DeleteTempVariable/*.csproj ./src/Maintenance.DeleteTempVariable/
COPY ./Maintenance.Apps/*.csproj ./Maintenance.Apps/

RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN dotnet restore Maintenance.Apps/Maintenance.Apps.csproj

COPY ./src/. ./src/
COPY ./Maintenance.Apps/. ./Maintenance.Apps/

RUN ls /src -l
WORKDIR /src/Maintenance.Apps
RUN dotnet publish -c release -o /app
FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS base

WORKDIR /app
COPY --from=builder /app .

ENV TZ="America/La_Paz"
ENV COMPlus_EnableDiagnostics=0

# Don t run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

ENTRYPOINT ["dotnet", "Maintenance.Apps.dll"]