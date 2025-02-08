# Base image: .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project file and restore dependencies
COPY Interface/*.csproj ./Interface/
RUN dotnet restore Interface/Interface.csproj

# Copy the remaining source code and build the application
COPY Interface/ ./Interface/
WORKDIR /app/Interface
RUN dotnet publish -c Release -o /out

# Base image: .NET Runtime for running the application
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Set the entry point
ENTRYPOINT ["dotnet", "Interface.dll"]
