# Etapa de build: compila el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish ./Rimovie/Rimovie.csproj -c Release -o out

# Etapa final: ejecuta la app publicada
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Rimovie.dll"]