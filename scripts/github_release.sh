#!/bin/bash
set -e

mkdir ../release

dotnet --info > ../release/dotnet_info.txt

find ../src/Fastenshtein/bin/Release -name "Fastenshtein.*.nupkg" -exec cp "{}" ../release/Fastenshtein.nupkg \;
find ../src/Fastenshtein/bin/Release -name "Fastenshtein.*.snupkg" -exec cp "{}" ../release/Fastenshtein.snupkg \;

gh release create $1 \
  "../release/Fastenshtein.nupkg" \
  "../release/Fastenshtein.snupkg" \
  "../coverage.net48.info#Code coverage report net48" \
  "../coverage.net8.info#Code coverage report net8" \
  "../release/dotnet_info.txt#Built with" \
  --draft \
  --generate-notes