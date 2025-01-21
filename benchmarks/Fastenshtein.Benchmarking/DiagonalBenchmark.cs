using BenchmarkDotNet.Attributes;

namespace Fastenshtein.Benchmarking;

[DisassemblyDiagnoser()]
[RankColumn]
public class DiagonalBenchmark
{
    private string[] _words;

    [GlobalSetup]
    public void SetUp() => _words = RandomWords.Create(20, 2_000);

    ////[Benchmark]
    ////public int DistanceQuickCosting()
    ////{
    ////    var value = 0;
    ////    var words = _words;
    ////    for (int i = 0; i < words.Length; i++)
    ////    {
    ////        for (int j = 0; j < words.Length; j++)
    ////        {
    ////            value = DiagonalStaticLevenshtein.DistanceQuickCosting(words[i], words[j]);
    ////        }
    ////    }
    ////    return value;
    ////}

    [Benchmark]
    public int DistanceSSE9()
    {
        var value = 0;
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                value = DiagonalStaticLevenshtein.DistanceSSE9(words[i], words[j]);
            }
        }
        return value;
    }

    [Benchmark]
    public int DistanceSSE7()
    {
        var value = 0;
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                value = DiagonalStaticLevenshtein.DistanceSSE7(words[i], words[j]);
            }
        }
        return value;
    }


    ////[Benchmark]
    ////public int DistanceSSE()
    ////{
    ////    var value = 0;
    ////    var words = _words;
    ////    for (int i = 0; i < words.Length; i++)
    ////    {
    ////        for (int j = 0; j < words.Length; j++)
    ////        {
    ////            value = DiagonalStaticLevenshtein.DistanceSSE(words[i], words[j]);
    ////        }
    ////    }
    ////    return value;
    ////}

    ////[Benchmark]
    ////public int Diagonal()
    ////{
    ////    var value = 0;
    ////    var words = _words;
    ////    for (int i = 0; i < words.Length; i++)
    ////    {
    ////        for (int j = 0; j < words.Length; j++)
    ////        {
    ////            value = DiagonalStaticLevenshtein.Distance(words[i], words[j]);
    ////        }
    ////    }
    ////    return value;
    ////}

    ////[Benchmark(Baseline = true)]
    ////public int FastenshteinStatic()
    ////{
    ////    var value = 0;
    ////    var words = _words;
    ////    for (int i = 0; i < words.Length; i++)
    ////    {
    ////        for (int j = 0; j < words.Length; j++)
    ////        {
    ////            global::Fastenshtein.Levenshtein.Distance(words[i], words[j]);
    ////        }
    ////    }
    ////    return value;
    ////}

    [Benchmark]
    public int Quickenshtein()
    {
        var value = 0;
        var words = _words;
        for (int i = 0; i < words.Length; i++)
        {
            for (int j = 0; j < words.Length; j++)
            {
                global::Quickenshtein.Levenshtein.GetDistance(words[i], words[j]);
            }
        }
        return value;
    }
}

