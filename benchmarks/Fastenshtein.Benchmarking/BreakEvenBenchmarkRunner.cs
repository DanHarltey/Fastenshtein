namespace Fastenshtein.Benchmarking;

using System;
using System.Diagnostics;

public static class BreakEvenBenchmarkRunner
{
    public static void Run()
    {
        Console.WriteLine("# Fastenshtein Break-Even Analysis: Full Calculation vs Early-Exit");
        Console.WriteLine("Exploring where Early-Exit is faster vs where Full Calculation is faster.");
        Console.WriteLine();

        // 1. Fixed Word Length N = 20, Sweep K from 1 to 20
        RunKSweepForLength(20, new[] { 1, 2, 3, 5, 7, 10, 15, 20 });

        // 2. Fixed Word Length N = 50, Sweep K from 1 to 50
        RunKSweepForLength(50, new[] { 1, 2, 5, 10, 15, 20, 25, 35, 50 });

        // 3. Fixed Word Length N = 100, Sweep K from 1 to 100
        RunKSweepForLength(100, new[] { 1, 2, 5, 10, 20, 30, 50, 75, 100 });

        // 4. Fixed Word Length N = 10, Sweep K from 1 to 10
        RunKSweepForLength(10, new[] { 1, 2, 3, 5, 7, 10 });

        // 5. Short Word Length N = 5, Sweep K from 1 to 5
        RunKSweepForLength(5, new[] { 1, 2, 3, 4, 5 });
    }

    private static void RunKSweepForLength(int wordLength, int[] kValues)
    {
        Console.WriteLine($"## Word Length N = {wordLength}");
        Console.WriteLine();

        int numQueries = 100;
        int numTargets = 100;
        int totalOps = numQueries * numTargets; // 10,000 ops
        int iterations = 10;

        string[] queries = RandomWords.Create(numQueries, wordLength);
        string[] divergentTargets = RandomWords.Create(numTargets, wordLength);

        // Warmup JIT
        for (int i = 0; i < 50; i++)
        {
            var lev = new global::Fastenshtein.Levenshtein(queries[i % numQueries]);
            lev.DistanceFrom(divergentTargets[i % numTargets]);
            lev.DistanceFrom(divergentTargets[i % numTargets], 2);
        }

        // Measure Full Baseline (Divergent)
        double fullDivergentMs = MeasureTime(() =>
        {
            for (int i = 0; i < queries.Length; i++)
            {
                var lev = new global::Fastenshtein.Levenshtein(queries[i]);
                for (int j = 0; j < divergentTargets.Length; j++)
                {
                    lev.DistanceFrom(divergentTargets[j]);
                }
            }
        }, iterations);

        // Table 1: Divergent Words
        Console.WriteLine($"### Scenario A: Divergent / Different Words (Length = {wordLength})");
        Console.WriteLine($"*Full Baseline: {fullDivergentMs:F2} ms ({fullDivergentMs * 1_000_000.0 / totalOps:F1} ns/op)*");
        Console.WriteLine();
        Console.WriteLine("| Threshold K | K / N Ratio | Full (10k ops) | Early-Exit (10k ops) | Speedup (Full / EE) | Faster Variant |");
        Console.WriteLine("| :---: | :---: | :---: | :---: | :---: | :---: |");

        foreach (int k in kValues)
        {
            double eeMs = MeasureTime(() =>
            {
                for (int i = 0; i < queries.Length; i++)
                {
                    var lev = new global::Fastenshtein.Levenshtein(queries[i]);
                    for (int j = 0; j < divergentTargets.Length; j++)
                    {
                        lev.DistanceFrom(divergentTargets[j], k);
                    }
                }
            }, iterations);

            double speedup = fullDivergentMs / eeMs;
            double ratio = (double)k / wordLength * 100.0;
            string winner = speedup >= 1.05 ? "**Early-Exit**" : (speedup <= 0.95 ? "**Full**" : "~Even");

            Console.WriteLine($"| K = {k,2} | {ratio,5:F1}% | {fullDivergentMs:F2} ms | {eeMs:F2} ms | **{speedup:F2}x** | {winner} |");
        }
        Console.WriteLine();

        // Table 2: Near Matches (Distance <= K)
        // Here, we create target words that have at most K edits, forcing the banded algorithm to compute until the end
        Console.WriteLine($"### Scenario B: Near Matches (Target distance <= K, Length = {wordLength})");
        Console.WriteLine();
        Console.WriteLine("| Threshold K | K / N Ratio | Full (10k ops) | Early-Exit (10k ops) | Speedup (Full / EE) | Faster Variant |");
        Console.WriteLine("| :---: | :---: | :---: | :---: | :---: | :---: |");

        var rnd = new Random(12345);

        foreach (int k in kValues)
        {
            string[] nearTargets = new string[numTargets];
            for (int i = 0; i < numTargets; i++)
            {
                char[] chars = queries[i % numQueries].ToCharArray();
                int edits = rnd.Next(0, Math.Min(k, wordLength) + 1);
                for (int e = 0; e < edits; e++)
                {
                    chars[rnd.Next(0, chars.Length)] = (char)('a' + rnd.Next(0, 26));
                }
                nearTargets[i] = new string(chars);
            }

            double fullNearMs = MeasureTime(() =>
            {
                for (int i = 0; i < queries.Length; i++)
                {
                    var lev = new global::Fastenshtein.Levenshtein(queries[i]);
                    for (int j = 0; j < nearTargets.Length; j++)
                    {
                        lev.DistanceFrom(nearTargets[j]);
                    }
                }
            }, iterations);

            double eeNearMs = MeasureTime(() =>
            {
                for (int i = 0; i < queries.Length; i++)
                {
                    var lev = new global::Fastenshtein.Levenshtein(queries[i]);
                    for (int j = 0; j < nearTargets.Length; j++)
                    {
                        lev.DistanceFrom(nearTargets[j], k);
                    }
                }
            }, iterations);

            double speedup = fullNearMs / eeNearMs;
            double ratio = (double)k / wordLength * 100.0;
            string winner = speedup >= 1.05 ? "**Early-Exit**" : (speedup <= 0.95 ? "**Full**" : "~Even");

            Console.WriteLine($"| K = {k,2} | {ratio,5:F1}% | {fullNearMs:F2} ms | {eeNearMs:F2} ms | **{speedup:F2}x** | {winner} |");
        }

        Console.WriteLine();
    }

    private static double MeasureTime(Action action, int iterations)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        long start = Stopwatch.GetTimestamp();
        for (int i = 0; i < iterations; i++)
        {
            action();
        }
        long stop = Stopwatch.GetTimestamp();

        return (double)(stop - start) * 1000.0 / Stopwatch.Frequency / iterations;
    }
}

