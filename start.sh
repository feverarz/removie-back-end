#!/bin/bash

# Compila el proyecto en modo Release y publica los binarios en ./out
dotnet publish ./Rimovie/Rimovie.csproj -c Release -o out

# Ejecuta el DLL generado
dotnet out/Rimovie.dll