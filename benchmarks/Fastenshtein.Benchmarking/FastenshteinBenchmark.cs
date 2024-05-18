using BenchmarkDotNet.Attributes;

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

    [Benchmark(Baseline = true)]
    public void Fastenshtein_1_0_0_8()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            var levenshtein = new global::Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_0_8(words[i]);

            for (int j = 0; j < words.Length; j++)
            {
                levenshtein.DistanceFrom(words[j]);
            }
        }
    }

    [Benchmark]
    public void FastenshteinStatic_1_0_0_8()
    {
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                global::Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_0_8.Distance(words[i], words[j]);
            }
        }
    }
}
