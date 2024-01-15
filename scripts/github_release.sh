#!/bin/bash
set -e

mkdir ../release

dotnet --info > ../release/dotnet_info.txt

find ../tests/Fastenshtein.Tests/TestResults -name coverage.info -exec cp "{}" ../release  \;

find ../src/Fastenshtein/bin/Release -name "Fastenshtein.*.nupkg" -exec cp "{}" ../release/Fastenshtein.nupkg \;
find ../src/Fastenshtein/bin/Release -name "Fastenshtein.*.snupkg" -exec cp "{}" ../release/Fastenshtein.snupkg \;

gh release create $1 \
  "../release/Fastenshtein.nupkg" \
  "../release/Fastenshtein.snupkg" \
  "../release/coverage.info#Code coverage report" \
  "../release/dotnet_info.txt#Built with" \
  --draft \
  --generate-notes