namespace Fastenshtein.Benchmarking;

using BenchmarkDotNet.Running;
using System;

static class Program
{
    /*
     * If you want to add your method to the benchmarking.
     * There are just one file that needs altering 
     * CompetitiveSingleThreadBenchmark.
     */

    static void Main(string[] args) 
        => BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(args);
}