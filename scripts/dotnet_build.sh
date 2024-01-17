#!/bin/bash
set -e

rm -rf ../release
mkdir ../release

dotnet --info > ../release/dotnet_info.txt

dotnet restore ../
dotnet build ../ --configuration Release --no-restore -p:ContinuousIntegrationBuild=true

if [[ $1 = "code_coverage" ]]; then
  dotnet test ../ --configuration Release --no-build --verbosity normal --framework net8.0 --collect:"XPlat Code Coverage;Format=lcov"
  find ../tests/Fastenshtein.Tests/TestResults/ -name "coverage.info" -type f -exec mv {} ../release/coverage.net8.info \;

  dotnet test ../ --configuration Release --no-build --verbosity normal --framework net48 --collect:"XPlat Code Coverage;Format=lcov"
  find ../tests/Fastenshtein.Tests/TestResults/ -name "coverage.info" -type f -exec mv {} ../release/coverage.net48.info \;
else
  dotnet test ../ --configuration Release --no-build --verbosity normal
fi

dotnet pack ../ --configuration Release --no-build

cp ../src/Fastenshtein/bin/Release/Fastenshtein.*.nupkg ../release/Fastenshtein.nupkg
cp ../src/Fastenshtein/bin/Release/Fastenshtein.*.snupkg ../release/Fastenshtein.snupkg