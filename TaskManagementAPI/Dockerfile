# Base image used for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install migration tools
RUN dotnet tool install --global dotnet-ef

# Copy the csproj file from the root
COPY ["TaskManagementAPI.csproj", "./"]

# Restore dependencies based on the csproj
RUN dotnet restore "./TaskManagementAPI.csproj"

# Copy the entire source code, including migrations
COPY . .

# Build the project in Release mode
WORKDIR "/src"
RUN dotnet build "./TaskManagementAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "./TaskManagementAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy the published app from the previous stage
COPY --from=publish /app/publish .

# Entry point to start the app, with optional migration flag
ENTRYPOINT ["dotnet", "TaskManagementAPI.dll"]
CMD ["dotnet", "ef", "database", "update", "--no-build"]
