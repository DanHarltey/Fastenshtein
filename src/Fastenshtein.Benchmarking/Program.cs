namespace Fastenshtein.Benchmarking
{
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

            var summary = BenchmarkRunner.Run<BenchmarkSmallWordsSingleThread>();
            summary = BenchmarkRunner.Run<BenchmarkNormalWordsSingleThread>();
            summary = BenchmarkRunner.Run<BenchmarkLargeWordsSingleThread>();

            summary = BenchmarkRunner.Run<BenchmarkSmallWordsMultiThread>();
            summary = BenchmarkRunner.Run<BenchmarkNormalWordsMultiThread>();
            summary = BenchmarkRunner.Run<BenchmarkLargeWordsMultiThread>();

            Console.WriteLine("Completed in : " + (DateTime.UtcNow - startTime));
        }
    }
}