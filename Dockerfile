FROM hub.nexdev.net/base/dotnet/core/sdk:7.0-alpine AS builder
WORKDIR /src

COPY ./src/Maintenance.DeleteTempVariable/. ./src/Maintenance.DeleteTempVariable
COPY ./Maintenance.Apps/Maintenance.Apps.csproj ./Maintenance.Apps/Maintenance.Apps.csproj

WORKDIR /src/Maintenance.Apps
RUN dotnet nuget add source http://nuget.nexdev.net/api/v2
RUN dotnet restore -v diag

COPY ./Maintenance.Apps .

RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app 

FROM hub.nexdev.net/base/dotnet/core/aspnet:7.0 AS base

WORKDIR /app
COPY --from=builder /app .

ENV TZ="America/La_Paz" 

# Don’t run as root
RUN adduser --disabled-password \
    --home /app \
    --gecos '' dotnetuser && chown -R dotnetuser /app
USER dotnetuser
#

ENTRYPOINT ["dotnet", "Maintenance.Apps.dll"]