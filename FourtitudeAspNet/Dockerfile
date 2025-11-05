# Base image (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project file and restore dependencies
COPY ["FourtitudeAspNet/FourtitudeAspNet.csproj", "FourtitudeAspNet/"]
RUN dotnet restore "FourtitudeAspNet/FourtitudeAspNet.csproj"

# Copy all source code
COPY . .
WORKDIR "/src/FourtitudeAspNet"
RUN dotnet build "FourtitudeAspNet.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the app
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FourtitudeAspNet.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime stage
FROM base AS final

# --- Create writable data directory ---
USER root
# Create /data for SQLite file storage
RUN mkdir -p /data && chown app /data
# Create /app/data if you prefer to use relative paths
RUN mkdir -p /app/data && chown app /app/data
# Create /logs for log file storage
RUN mkdir -p /logs && chown app /logs
USER app

# --- Declare volume for persistence ---
VOLUME ["/data", "/logs"]

WORKDIR /app
COPY --from=publish /app/publish .

# --- Start the app ---
ENTRYPOINT ["dotnet", "FourtitudeAspNet.dll"]
