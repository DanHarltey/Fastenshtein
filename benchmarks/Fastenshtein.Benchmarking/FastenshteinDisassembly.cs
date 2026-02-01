using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using System.Diagnostics.CodeAnalysis;

namespace Fastenshtein.Benchmarking;

[DisassemblyDiagnoser()]
public class FastenshteinDisassembly
{
    [Benchmark]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Must be an instance method for BenchmarkDotNet")]
    public int Fastenshtein()
    {
        var testWords = RandomWords.Create(2, 8);
        var levenshtein = new global::Fastenshtein.Levenshtein(testWords[0]);
        return levenshtein.DistanceFrom(testWords[1]);
    }

    [Benchmark(Baseline = true)]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Must be an instance method for BenchmarkDotNet")]
    public int Fastenshtein_1_0_0_12()
    {
        var testWords = RandomWords.Create(2, 8);
        var levenshtein = new global::Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_0_12(testWords[0]);
        return levenshtein.DistanceFrom(testWords[1]);
    }

    [Benchmark]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Must be an instance method for BenchmarkDotNet")]
    public int FastenshteinStatic()
    {
        var testWords = RandomWords.Create(2, 8);
        return global::Fastenshtein.Levenshtein.Distance(testWords[0], testWords[1]);
    }

    [Benchmark]
    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Must be an instance method for BenchmarkDotNet")]
    public int FastenshteinStatic_1_0_0_12()
    {
        var testWords = RandomWords.Create(2, 8);
        return global::Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_0_12.Distance(testWords[0], testWords[1]);
    }
}
