using BenchmarkDotNet.Attributes;
using System;

namespace Fastenshtein.Benchmarking;

[RankColumn]
public abstract class FastenshteinBenchmark
{
    private string[] _words;

    protected abstract string[] CreateTestData();

    [GlobalSetup]
    public void SetUp() => _words = CreateTestData();

    [Benchmark]
    public void Fastenshtein()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new global::Fastenshtein.Levenshtein(words[i]);

            for (int j = 0; j < words.Length; j++)
            {
                levenshtein.DistanceFrom(words[j]);
            }
        }
    }

    [Benchmark]
    public void FastenshteinValue()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new global::Fastenshtein.LevenshteinValue<char>(words[i]);

            for (int j = 0; j < words.Length; j++)
            {
                levenshtein.DistanceFrom(words[j].AsSpan());
            }
        }
    }

    [Benchmark]
    public void FastenshteinValueAdvanced()
    {
        Span<int> buffer = stackalloc int[400];
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new global::Fastenshtein.LevenshteinValue<char>(words[i], buffer);

            for (int j = 0; j < words.Length; j++)
            {
                levenshtein.DistanceFrom(words[j].AsSpan());
            }
        }
    }

    [Benchmark]
    public void FastenshteinStatic()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                global::Fastenshtein.Levenshtein.Distance(words[i], words[j]);
            }
        }
    }

    [Benchmark]
    public void FastenshteinStaticSpan()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                global::Fastenshtein.Levenshtein.Distance(words[i].AsSpan(), words[j].AsSpan());
            }
        }
    }

    [Benchmark(Baseline = true)]
    public void Fastenshtein_1_0_12()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new global::Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_12(words[i]);

            for (int j = 0; j < words.Length; j++)
            {
                levenshtein.DistanceFrom(words[j]);
            }
        }
    }

    [Benchmark]
    public void FastenshteinStatic_1_0_12()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                global::Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_12.Distance(words[i], words[j]);
            }
        }
    }
}
