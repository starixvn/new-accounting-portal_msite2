FROM hub.nexdev.net/base/dotnet/core/sdk:6.0 AS builder

WORKDIR /src

COPY ./src/Maintenance.DeleteTempVariable/. ./src/Maintenance.DeleteTempVariable
COPY ./src/ControlSyncService/. ./src/ControlSyncService
COPY ./Maintenance.Apps/Maintenance.Apps.csproj ./Maintenance.Apps/Maintenance.Apps.csproj

WORKDIR /src/Maintenance.Apps
RUN dotnet nuget add source http://nuget.nexdev.net/api/v2
RUN dotnet restore -v diag

COPY ./Maintenance.Apps .

RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app 

FROM hub.nexdev.net/base/dotnet/core/aspnet:6.0 AS base

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

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