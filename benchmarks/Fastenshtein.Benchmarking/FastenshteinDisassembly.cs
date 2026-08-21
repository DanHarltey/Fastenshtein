using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using System;

namespace Fastenshtein.Benchmarking;

[DisassemblyDiagnoser()]
public class FastenshteinDisassembly
{
    [Benchmark]
    public int Fastenshtein()
    {
        var testWords = RandomWords.Create(2, 8);
        var levenshtein = new global::Fastenshtein.Levenshtein(testWords[0]);
        return levenshtein.DistanceFrom(testWords[1]);
    }

    [Benchmark]
    public int FastenshteinValue()
    {
        var testWords = RandomWords.Create(2, 8);
        var levenshtein = new global::Fastenshtein.LevenshteinValue<char>(testWords[0]);
        return levenshtein.DistanceFrom(testWords[1].AsSpan());
    }

    [Benchmark]
    public int FastenshteinValueAdvanced()
    {
        var testWords = RandomWords.Create(2, 8);
        Span<int> buffer = stackalloc int[8];
        var levenshtein = new global::Fastenshtein.LevenshteinValue<char>(testWords[0], buffer);
        return levenshtein.DistanceFrom(testWords[1].AsSpan());
    }

    [Benchmark(Baseline = true)]
    public int Fastenshtein_1_0_12()
    {
        var testWords = RandomWords.Create(2, 8);
        var levenshtein = new global::Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_12(testWords[0]);
        return levenshtein.DistanceFrom(testWords[1]);
    }

    [Benchmark]
    public int FastenshteinStatic()
    {
        var testWords = RandomWords.Create(2, 8);
        return global::Fastenshtein.Levenshtein.Distance(testWords[0], testWords[1]);
    }

    [Benchmark]
    public int FastenshteinStaticSpan()
    {
        var testWords = RandomWords.Create(2, 8);
        return global::Fastenshtein.Levenshtein.Distance(testWords[0].AsSpan(), testWords[1].AsSpan());
    }


    [Benchmark]
    public int FastenshteinStatic_1_0_12()
    {
        var testWords = RandomWords.Create(2, 8);
        return global::Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_12.Distance(testWords[0], testWords[1]);
    }
}
