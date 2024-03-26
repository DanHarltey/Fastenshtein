using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using System.Diagnostics.CodeAnalysis;

namespace Fastenshtein.Benchmarking
{
    [DisassemblyDiagnoser(/*printSource: true,*/), MemoryDiagnoser]
    public class FastenshteinDisassembly
    {
        [Benchmark]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Must be an instance method for BenchmarkDotNet")]
        public int Fastenshtein()
        {
            var levenshtein = new global::Fastenshtein.Levenshtein("test");
            return levenshtein.DistanceFrom("test");
        }

        [Benchmark]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Must be an instance method for BenchmarkDotNet")]
        public int Fastenshtein3()
        {
            var levenshtein = new global::Fastenshtein.Levenshtein("test");
            return levenshtein.DistanceFrom3("test");
        }

        [Benchmark]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Must be an instance method for BenchmarkDotNet")]
        public int Fastenshtein_Inc()
        {
            var levenshtein = new global::Fastenshtein.Levenshtein("test");
            return levenshtein.DistanceFrom_Inc("test");
        }


        ////[Benchmark(Baseline = true)]
        ////[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Must be an instance method for BenchmarkDotNet")]
        ////public int Fastenshtein_1_0_0_8()
        ////{
        ////    var levenshtein = new global::Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_0_8("test");
        ////    return levenshtein.DistanceFrom("test");
        ////}
    }
}
