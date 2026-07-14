FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MonitoraAgro.API/MonitoraAgro.API.csproj", "src/MonitoraAgro.API/"]
RUN dotnet restore "src/MonitoraAgro.API/MonitoraAgro.API.csproj"
COPY . .
WORKDIR "/src/src/MonitoraAgro.API"
RUN dotnet build "MonitoraAgro.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MonitoraAgro.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonitoraAgro.API.dll"]