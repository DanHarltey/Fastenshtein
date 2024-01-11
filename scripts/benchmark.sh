#!/bin/bash

# Stop on any errors
set -e

dotnet restore ../
dotnet build ../ --configuration Release --nologo --no-restore
dotnet test ../tests/Fastenshtein.Tests --configuration Release --framework net8.0 --nologo --no-build --verbosity normal
dotnet ../benchmarks/Fastenshtein.Benchmarking/bin/Release/net8.0/Fastenshtein.Benchmarking.dll