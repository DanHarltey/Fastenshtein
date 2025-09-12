#!/bin/bash
set -e

rm -rf ../release
mkdir ../release

dotnet --info > ../release/dotnet_info.txt

dotnet restore ../
dotnet build ../ --configuration Release --no-restore -p:ContinuousIntegrationBuild=true

if [ $1 = "test_coverage" ] 
then
  dotnet test ../ --configuration Release --no-build --verbosity normal --framework net9.0 --collect:"Code Coverage;Format=cobertura" --settings:"../.runsettings"
  find ../tests/Fastenshtein.Tests/TestResults/ -name "*.cobertura.xml" -type f -exec mv {} ../release/coverage.net9.xml \;

  dotnet test ../ --configuration Release --no-build --framework net481 --collect:"Code Coverage;Format=cobertura" --settings:"../.runsettings"
  find ../tests/Fastenshtein.Tests/TestResults/ -name "*.cobertura.xml" -type f -exec mv {} ../release/coverage.net481.xml \;

  xxd -plain -u ../src/Fastenshtein/bin/Release/net462/Fastenshtein.dll | tr -d '\n' > ../release/Fastenshtein_net462.hex

elif [ $1 = "no_net481" ]
then
  dotnet test ../ --configuration Release --no-build --verbosity normal --framework net9.0
else
  dotnet test ../ --configuration Release --no-build --verbosity normal
fi

dotnet pack ../ --configuration Release --no-build

cp ../src/Fastenshtein/bin/Release/Fastenshtein.*.nupkg ../release/Fastenshtein.nupkg
cp ../src/Fastenshtein/bin/Release/Fastenshtein.*.snupkg ../release/Fastenshtein.snupkg