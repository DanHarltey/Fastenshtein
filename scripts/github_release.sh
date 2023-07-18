#!/bin/bash
set -e

gh release create $1 \
  "../release/Fastenshtein.nupkg" \
  "../release/Fastenshtein.snupkg" \
  "../release/coverage.net48.info#Code coverage report net48" \
  "../release/coverage.net8.info#Code coverage report net8" \
  "../release/dotnet_info.txt#Built with" \
  --draft \
  --generate-notes