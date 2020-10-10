namespace Fastenshtein.Benchmarking
{
    using BenchmarkDotNet.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Text;

    [DisassemblyDiagnoser(/*printSource: true,*/)]
    public class BenchmarkFastenshteinDisassembly
    {
        [Benchmark(Baseline = true)]
        public void Fastenshtein()
        {
            var levenshtein = new global::Fastenshtein.Levenshtein("test");
            levenshtein.DistanceFrom("test");
        }

        [Benchmark]
        public void Fastenshtein_1_0_0_5()
        {
            var levenshtein = new global::Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_0_5("test");
            levenshtein.DistanceFrom("test");
        }
    }
}
