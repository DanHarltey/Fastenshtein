#!/bin/bash
set -e

gh release create $1 \
  "../release/Fastenshtein.nupkg" \
  "../release/Fastenshtein.snupkg" \
  "../release/coverage.net481.info#Code coverage report net481" \
  "../release/coverage.net9.info#Code coverage report net9" \
  "../release/dotnet_info.txt#Built with" \
  --draft \
  --generate-notes