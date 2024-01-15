#!/bin/bash
set -e

dotnet restore ../
dotnet build ../ --configuration Release --no-restore /p:ContinuousIntegrationBuild=true

if [[ $1 = "code_coverage" ]]; then
  dotnet test ../ --configuration Release --no-build --verbosity normal --collect:"XPlat Code Coverage;Format=lcov"
else
  dotnet test ../ --configuration Release --no-build --verbosity normal
fi

dotnet pack ../ --configuration Release --no-build