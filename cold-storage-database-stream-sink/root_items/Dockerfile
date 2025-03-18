FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080 8081
ENV DOTNET_ENVIRONMENT=Development-docker

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["cold-storage-database-stream-sink.csproj", "."]
RUN dotnet restore "./cold-storage-database-stream-sink.csproj"

COPY . .
RUN dotnet publish "./cold-storage-database-stream-sink.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "cold-storage-database-stream-sink.dll"]
