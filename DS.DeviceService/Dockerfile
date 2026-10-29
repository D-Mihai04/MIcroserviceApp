# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /App

# Copy project files
COPY . ./

# Restore dependencies
RUN dotnet restore

# Build and publish
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /App

# Copy published application
COPY --from=build /App/out .

# Start the application
ENTRYPOINT ["dotnet", "DS.DeviceService.dll"]
