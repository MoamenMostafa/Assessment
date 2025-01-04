# Use the official ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files for Management.API and its dependencies
COPY ["Management.API/Management.API.csproj", "Management.API/"]
COPY ["Management.BL/Management.BL.csproj", "Management.BL/"]
COPY ["Management.DL/Management.DL.csproj", "Management.DL/"]

# Restore dependencies for Management.API
RUN dotnet restore ./Management.API/Management.API.csproj

# Copy the entire solution into the container
COPY . .

# Build the application
WORKDIR /src/Management.API
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Use the runtime base image for the final container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Define the entry point for the application
ENTRYPOINT ["dotnet", "Management.API.dll"]