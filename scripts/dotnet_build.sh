#!/bin/bash
set -e

rm -rf ../release
mkdir ../release

dotnet --info 
dotnet --info > ../release/dotnet_info.txt

dotnet restore ../
dotnet build ../ --configuration Release --no-restore -p:ContinuousIntegrationBuild=true

if [ "${1:-}" = "test_coverage" ]
then
  dotnet test ../ --configuration Release --no-build --framework net11.0 --results-directory ../release -- --coverage --coverage-output-format cobertura --coverage-output coverage.net11.xml

  dotnet test ../ --configuration Release --no-build --framework net481 --results-directory ../release -- --coverage --coverage-output-format cobertura --coverage-output coverage.net481.xml

  xxd -plain -u ../src/Fastenshtein/bin/Release/net462/Fastenshtein.dll | tr -d '\n' > ../release/Fastenshtein_net462.hex

elif [ "${1:-}" = "no_net481" ]
then
  echo "==> Starting tests for net10.0"
  dotnet test ../ --configuration Release --no-build --framework net10.0
  echo "==> Starting tests for net11.0"
  dotnet test ../ --configuration Release --no-build --framework net11.0
  echo "==> Finished tests for net10.0 and net11.0"
else
  echo "==> Starting tests for net10.0"
  dotnet test ../ --configuration Release --no-build --framework net10.0
  echo "==> Starting tests for net11.0"
  dotnet test ../ --configuration Release --no-build --framework net11.0
  echo "==> Starting tests for net481"
  dotnet test ../ --configuration Release --no-build --framework net481
  echo "==> Finished tests for net10.0, net11.0 and net481"
fi

dotnet pack ../ --configuration Release --no-build

cp ../src/Fastenshtein/bin/Release/Fastenshtein.*.nupkg ../release/Fastenshtein.nupkg
cp ../src/Fastenshtein/bin/Release/Fastenshtein.*.snupkg ../release/Fastenshtein.snupkg
