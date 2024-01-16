#!/bin/bash
set -e

dotnet restore ../
dotnet build ../ --configuration Release --no-restore /p:ContinuousIntegrationBuild=true

if [[ $1 = "code_coverage" ]]; then
  dotnet test ../ --configuration Release --no-build --verbosity normal --framework net8.0 --collect:"XPlat Code Coverage;Format=lcov"
  find -name "coverage.info" -type f -exec mv {} ../coverage.net8.info \;

  dotnet test ../ --configuration Release --no-build --verbosity normal --framework net48 --collect:"XPlat Code Coverage;Format=lcov"
  find -name "coverage.info" -type f -exec mv {} ../coverage.net48.info \;
else
  dotnet test ../ --configuration Release --no-build --verbosity normal
fi

dotnet pack ../ --configuration Release --no-build