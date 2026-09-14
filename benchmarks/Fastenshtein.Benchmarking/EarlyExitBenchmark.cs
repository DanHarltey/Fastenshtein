namespace Fastenshtein.Benchmarking;

using BenchmarkDotNet.Attributes;
using System;

[RankColumn, MemoryDiagnoser]
public class EarlyExitBenchmark
{
    private string[] _words;
    private string[] _nearMatches;
    private const int MaxDistance = 2;

    [GlobalSetup]
    public void SetUp()
    {
        _words = RandomWords.Create(100, 20);
        _nearMatches = new string[_words.Length];
        var random = new Random(42);
        for (int i = 0; i < _words.Length; i++)
        {
            char[] chars = _words[i].ToCharArray();
            // 1 or 2 edits
            chars[random.Next(0, chars.Length)] = (char)('a' + random.Next(0, 26));
            if (random.Next(0, 2) == 1)
            {
                chars[random.Next(0, chars.Length)] = (char)('a' + random.Next(0, 26));
            }
            _nearMatches[i] = new string(chars);
        }
    }

    [Benchmark(Baseline = true)]
    public void Fastenshtein_Full()
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
    public void Fastenshtein_EarlyExit_DifferentWords()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new global::Fastenshtein.Levenshtein(words[i]);
            for (int j = 0; j < words.Length; j++)
            {
                levenshtein.DistanceFrom(words[j], MaxDistance);
            }
        }
    }

    [Benchmark]
    public void Fastenshtein_EarlyExit_NearMatches()
    {
        var words = _words;
        var near = _nearMatches;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new global::Fastenshtein.Levenshtein(words[i]);
            levenshtein.DistanceFrom(near[i], MaxDistance);
        }
    }

    [Benchmark]
    public void FastenshteinStatic_EarlyExit_DifferentWords()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                global::Fastenshtein.Levenshtein.Distance(words[i], words[j], MaxDistance);
            }
        }
    }

    [Benchmark]
    public void FastenshteinStaticSpan_EarlyExit_DifferentWords()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                global::Fastenshtein.Levenshtein.Distance(words[i].AsSpan(), words[j].AsSpan(), MaxDistance);
            }
        }
    }
}

