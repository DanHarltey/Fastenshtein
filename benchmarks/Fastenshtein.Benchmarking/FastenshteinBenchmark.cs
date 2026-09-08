using BenchmarkDotNet.Attributes;
using Fastenshtein.Benchmarking.FastenshteinOld;
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
    public void FastenshteinInstance()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new Levenshtein(words[i]);

            for (int j = 0; j < words.Length; j++)
            {
                levenshtein.DistanceFrom(words[j]);
            }
        }
    }

    [Benchmark(Baseline = true)]
    public void FastenshteinInstance_Old()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new Fastenshtein_1_1_1(words[i]);

            for (int j = 0; j < words.Length; j++)
            {
                levenshtein.DistanceFrom(words[j]);
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
                Levenshtein.Distance(words[i], words[j]);
            }
        }
    }


    [Benchmark]
    public void FastenshteinStatic_Old()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                Fastenshtein_1_1_1.Distance(words[i], words[j]);
            }
        }
    }

    [Benchmark]
    public void FastenshteinValueInstance()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new LevenshteinValue<char>(words[i]);

            for (int j = 0; j < words.Length; j++)
            {
                levenshtein.DistanceFrom(words[j].AsSpan());
            }
        }
    }

    [Benchmark]
    public void FastenshteinValueInstance_Old()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new FastenshteinValue_1_1_1<char>(words[i]);

            for (int j = 0; j < words.Length; j++)
            {
                levenshtein.DistanceFrom(words[j].AsSpan());
            }
        }
    }

    [Benchmark]
    public void FastenshteinValueSpanCtor()
    {
        Span<int> buffer = stackalloc int[400];
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new LevenshteinValue<char>(words[i], buffer);

            for (int j = 0; j < words.Length; j++)
            {
                levenshtein.DistanceFrom(words[j].AsSpan());
            }
        }
    }

    [Benchmark]
    public void FastenshteinValueSpanCtor_Old()
    {
        Span<int> buffer = stackalloc int[400];
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new FastenshteinValue_1_1_1<char>(words[i], buffer);

            for (int j = 0; j < words.Length; j++)
            {
                levenshtein.DistanceFrom(words[j].AsSpan());
            }
        }
    }

    [Benchmark]
    public void FastenshteinSpanStatic()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                Levenshtein.Distance(words[i].AsSpan(), words[j].AsSpan());
            }
        }
    }

    [Benchmark]
    public void FastenshteinSpanStatic_Old()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                Fastenshtein_1_1_1.Distance(words[i].AsSpan(), words[j].AsSpan());
            }
        }
    }
}
