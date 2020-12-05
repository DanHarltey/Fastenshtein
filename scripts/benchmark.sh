#!/bin/bash

# Stop on any errors
set -e

dotnet restore ../
dotnet build ../src/Fastenshtein --configuration Release --framework netstandard1.0 --nologo --no-restore
dotnet build ../benchmarks/Fastenshtein.Benchmarking --configuration Release --framework net5.0 --nologo --no-restore
dotnet ../benchmarks/Fastenshtein.Benchmarking/bin/Release/net5.0/Fastenshtein.Benchmarking.dll