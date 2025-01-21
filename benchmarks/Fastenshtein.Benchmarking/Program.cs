namespace Fastenshtein.Benchmarking;

using BenchmarkDotNet.Running;
using System;
using System.Numerics;
using System.Runtime.Intrinsics;

static class Program
{
    /*
     * If you want to add your method to the benchmarking. 
     * There are just two files that need altering 
     * BenchmarkMultiThread & BenchmarkSingleThread.
     */

    static void Main(string[] args)
    {
        if(Vector512.IsHardwareAccelerated)
        {
            Console.WriteLine("IsHardwareAccelerated");
        }

        if (Vector256.IsHardwareAccelerated)
        {
            Console.WriteLine("256 IsHardwareAccelerated");
        }

        if (Vector128.IsHardwareAccelerated)
        {
            Console.WriteLine("128 IsHardwareAccelerated");
        }


        DateTime startTime = DateTime.UtcNow;

        if (args.Length != 0 && string.Equals(args[0], "d", StringComparison.OrdinalIgnoreCase))
        {
            _ = BenchmarkRunner.Run<FastenshteinDisassembly>();
        }
        else if (args.Length != 0 && string.Equals(args[0], "c", StringComparison.OrdinalIgnoreCase))
        {
            _ = BenchmarkRunner.Run<CompetitiveBenchmarkSmallWordsSingleThread>();
            _ = BenchmarkRunner.Run<CompetitiveBenchmarkNormalWordsSingleThread>();
            _ = BenchmarkRunner.Run<CompetitiveBenchmarkLargeWordsSingleThread>();

            _ = BenchmarkRunner.Run<CompetitiveBenchmarkSmallWordsMultiThread>();
            _ = BenchmarkRunner.Run<CompetitiveBenchmarkNormalWordsMultiThread>();
            _ = BenchmarkRunner.Run<CompetitiveBenchmarkLargeWordsMultiThread>();
        }
        else if (args.Length != 0 && string.Equals(args[0], "f", StringComparison.OrdinalIgnoreCase))
        {
            _ = BenchmarkRunner.Run<BenchmarkSmallWordsSingleThread>();
            _ = BenchmarkRunner.Run<BenchmarkNormalWordsSingleThread>();
            _ = BenchmarkRunner.Run<BenchmarkLargeWordsSingleThread>();
        }
        else
        {
            _ = BenchmarkRunner.Run<DiagonalBenchmark>();
        }

        Console.WriteLine("Completed in : " + (DateTime.UtcNow - startTime));
    }
}