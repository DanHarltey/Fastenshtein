# Running benchmarks

Always run benchmarks in Release mode.

## Discover available benchmarks

dotnet run -c Release --project benchmarks/Fastenshtein.Benchmarking -- --list flat

## Run all disassembly benchmarks

dotnet run -c Release --project benchmarks/Fastenshtein.Benchmarking  -- --filter "*FastenshteinDisassembly*"

## Run one method disassembly benchmarks

dotnet run -c Release --project benchmarks/Fastenshtein.Benchmarking  -- --filter "*FastenshteinDisassembly.FastenshteinInstance*"

## Run comparison benchmarks

dotnet run -c Release --project benchmarks/Fastenshtein.Benchmarking -- --filter "*Competitive*"