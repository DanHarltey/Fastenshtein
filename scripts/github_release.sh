#!/bin/bash
set -e

gh release create $1 \
  "../release/Fastenshtein.nupkg" \
  "../release/Fastenshtein.snupkg" \
  "../release/coverage.net481.xml#Code coverage report net481" \
  "../release/coverage.net9.xml#Code coverage report net9" \
  "../release/dotnet_info.txt#Built with" \
  "../release/Fastenshtein_net462.hex#Fastenshtein SQL Assembly Hex" \
  --draft \
  --generate-notes