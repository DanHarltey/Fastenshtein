namespace Fastenshtein.Benchmarking
{
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Running;
    using System;

    static class Program
    {
        /*
         * If you want to add your method to the benchmarking. 
         * There are just two files that need altering 
         * BenchmarkMultiThread & BenchmarkSingleThread.
         */

        static void Main(string[] args)
        {
            DateTime startTime = DateTime.UtcNow;

            if (args.Length != 0 && string.Equals(args[0], "d", StringComparison.OrdinalIgnoreCase))
            {
                _ = BenchmarkRunner.Run<FastenshteinDisassembly>();
            }
            else
            { 
                _ = BenchmarkRunner.Run<BenchmarkSmallWordsSingleThread>();
                _ = BenchmarkRunner.Run<BenchmarkNormalWordsSingleThread>();
                _ = BenchmarkRunner.Run<BenchmarkLargeWordsSingleThread>();

                _ = BenchmarkRunner.Run<BenchmarkSmallWordsMultiThread>();
                _ = BenchmarkRunner.Run<BenchmarkNormalWordsMultiThread>();
                _ = BenchmarkRunner.Run<BenchmarkLargeWordsMultiThread>();
            }

            Console.WriteLine("Completed in : " + (DateTime.UtcNow - startTime));
        }
    }
}