using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using Fastenshtein.Benchmarking.FastenshteinOld;
using System;

namespace Fastenshtein.Benchmarking;

[DisassemblyDiagnoser()]
public class FastenshteinDisassembly
{
    private static readonly string[] TestWords = RandomWords.Create(2, 8);

    [Benchmark]
    public int FastenshteinInstance()
    {
        var levenshtein = new Levenshtein(TestWords[0]);
        return levenshtein.DistanceFrom(TestWords[1]);
    }

    [Benchmark(Baseline = true)]
    public int FastenshteinInstance_Old()
    {
        var levenshtein = new Fastenshtein_1_1_1(TestWords[0]);
        return levenshtein.DistanceFrom(TestWords[1]);
    }

    [Benchmark]
    public int FastenshteinStatic()
        => Levenshtein.Distance(TestWords[0], TestWords[1]);

    [Benchmark]
    public int FastenshteinStatic_Old()
        => Fastenshtein_1_1_1.Distance(TestWords[0], TestWords[1]);

    [Benchmark]
    public int FastenshteinValueInstance()
    {
        var levenshtein = new LevenshteinValue<char>(TestWords[0]);
        return levenshtein.DistanceFrom(TestWords[1].AsSpan());
    }

    [Benchmark]
    public int FastenshteinValueInstance_Old()
    {
        var levenshtein = new FastenshteinValue_1_1_1<char>(TestWords[0]);
        return levenshtein.DistanceFrom(TestWords[1].AsSpan());
    }

    [Benchmark]
    public int FastenshteinValueSpanCtor()
    {
        Span<int> buffer = stackalloc int[8];
        var levenshtein = new LevenshteinValue<char>(TestWords[0], buffer);
        return levenshtein.DistanceFrom(TestWords[1].AsSpan());
    }

    [Benchmark]
    public int FastenshteinValueSpanCtor_Old()
    {
        Span<int> buffer = stackalloc int[8];
        var levenshtein = new FastenshteinValue_1_1_1<char>(TestWords[0], buffer);
        return levenshtein.DistanceFrom(TestWords[1].AsSpan());
    }

    [Benchmark]
    public int FastenshteinSpanStatic() 
        =>  Levenshtein.Distance(TestWords[0].AsSpan(), TestWords[1].AsSpan());

    [Benchmark]
    public int FastenshteinSpanStatic_Old()
      => Fastenshtein_1_1_1.Distance(TestWords[0].AsSpan(), TestWords[1].AsSpan());
}
