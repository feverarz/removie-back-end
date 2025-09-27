#!/bin/bash
dotnet publish ./Rimovie/Rimovie.csproj -c Release -o ./Rimovie/out
dotnet ./Rimovie/out/Rimovie.dll
