# Usa la imagen oficial de .NET SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia el archivo de proyecto y restaura dependencias
COPY *.csproj .
RUN dotnet restore

# Copia el resto del código y compila
COPY . .
RUN dotnet publish -c Release -o /publish

# Usa una imagen más ligera para ejecutar el binario
FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR /app

# Copia el binario compilado
COPY --from=build /publish .

# Comando para ejecutar el CLI con argumentos
ENTRYPOINT ["dotnet", "Interface.dll"]
