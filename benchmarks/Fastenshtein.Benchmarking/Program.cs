namespace Fastenshtein.Benchmarking
{
    using BenchmarkDotNet.Reports;
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

            ////BenchmarkRunner.Run<BenchmarkFastenshteinDisassembly>();

            BenchmarkRunner.Run<BenchmarkSmallWordsSingleThread>();
            BenchmarkRunner.Run<BenchmarkNormalWordsSingleThread>();
            BenchmarkRunner.Run<BenchmarkLargeWordsSingleThread>();

            BenchmarkRunner.Run<BenchmarkSmallWordsMultiThread>();
            BenchmarkRunner.Run<BenchmarkNormalWordsMultiThread>();
            BenchmarkRunner.Run<BenchmarkLargeWordsMultiThread>();

            Console.WriteLine("Completed in : " + (DateTime.UtcNow - startTime));
        }
    }
}