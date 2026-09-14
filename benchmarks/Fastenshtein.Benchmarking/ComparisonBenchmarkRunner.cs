namespace Fastenshtein.Benchmarking;

using System;
using System.Diagnostics;

public static class ComparisonBenchmarkRunner
{
    private const int MaxDistance = 2;

    public static void RunAll()
    {
        Console.WriteLine("# Fastenshtein Benchmark: Full Calculation vs Early-Exit Mode");
        Console.WriteLine($"MaxDistance threshold: {MaxDistance}");
        Console.WriteLine();

        RunScenario("Normal Words (Length 20) - Divergent / Different Words", 20, false);
        RunScenario("Normal Words (Length 20) - Near Matches (Distance <= 2)", 20, true);

        RunScenario("Short Words (Length 5) - Divergent / Different Words", 5, false);
        RunScenario("Short Words (Length 5) - Near Matches (Distance <= 2)", 5, true);

        RunScenario("Long Words (Length 100) - Divergent / Different Words", 100, false);
        RunScenario("Long Words (Length 100) - Near Matches (Distance <= 2)", 100, true);
    }

    private static void RunScenario(string scenarioTitle, int wordLength, bool nearMatches)
    {
        Console.WriteLine($"### {scenarioTitle}");
        Console.WriteLine();

        int numQueries = 100;
        int numTargets = 100;
        int totalOperations = numQueries * numTargets; // 10,000 comparisons per iteration
        int iterations = 10;

        string[] queries = RandomWords.Create(numQueries, wordLength);
        string[] targets;

        if (nearMatches)
        {
            targets = new string[numTargets];
            var rnd = new Random(42);
            for (int i = 0; i < numTargets; i++)
            {
                char[] chars = queries[i % numQueries].ToCharArray();
                int edits = rnd.Next(0, MaxDistance + 1);
                for (int e = 0; e < edits; e++)
                {
                    chars[rnd.Next(0, chars.Length)] = (char)('a' + rnd.Next(0, 26));
                }
                targets[i] = new string(chars);
            }
        }
        else
        {
            targets = RandomWords.Create(numTargets, wordLength);
        }

        // Methods to compare
        var benchmarkCases = new (string MethodName, Action Action)[]
        {
            ("Instance.DistanceFrom (Full)", () =>
            {
                for (int i = 0; i < queries.Length; i++)
                {
                    var lev = new global::Fastenshtein.Levenshtein(queries[i]);
                    for (int j = 0; j < targets.Length; j++)
                    {
                        lev.DistanceFrom(targets[j]);
                    }
                }
            }),
            ("Instance.DistanceFrom (Early-Exit)", () =>
            {
                for (int i = 0; i < queries.Length; i++)
                {
                    var lev = new global::Fastenshtein.Levenshtein(queries[i]);
                    for (int j = 0; j < targets.Length; j++)
                    {
                        lev.DistanceFrom(targets[j], MaxDistance);
                    }
                }
            }),
            ("Static.Distance (Full)", () =>
            {
                for (int i = 0; i < queries.Length; i++)
                {
                    for (int j = 0; j < targets.Length; j++)
                    {
                        global::Fastenshtein.Levenshtein.Distance(queries[i], targets[j]);
                    }
                }
            }),
            ("Static.Distance (Early-Exit)", () =>
            {
                for (int i = 0; i < queries.Length; i++)
                {
                    for (int j = 0; j < targets.Length; j++)
                    {
                        global::Fastenshtein.Levenshtein.Distance(queries[i], targets[j], MaxDistance);
                    }
                }
            }),
            ("Span.Distance (Full)", () =>
            {
                for (int i = 0; i < queries.Length; i++)
                {
                    var qSpan = queries[i].AsSpan();
                    for (int j = 0; j < targets.Length; j++)
                    {
                        global::Fastenshtein.Levenshtein.Distance(qSpan, targets[j].AsSpan());
                    }
                }
            }),
            ("Span.Distance (Early-Exit)", () =>
            {
                for (int i = 0; i < queries.Length; i++)
                {
                    var qSpan = queries[i].AsSpan();
                    for (int j = 0; j < targets.Length; j++)
                    {
                        global::Fastenshtein.Levenshtein.Distance(qSpan, targets[j].AsSpan(), MaxDistance);
                    }
                }
            }),
            ("AutoComplete.Distance (Full)", () =>
            {
                for (int i = 0; i < queries.Length; i++)
                {
                    for (int j = 0; j < targets.Length; j++)
                    {
                        global::Fastenshtein.AutoCompleteLevenshtein.Distance(queries[i], targets[j]);
                    }
                }
            }),
            ("AutoComplete.Distance (Early-Exit)", () =>
            {
                for (int i = 0; i < queries.Length; i++)
                {
                    for (int j = 0; j < targets.Length; j++)
                    {
                        global::Fastenshtein.AutoCompleteLevenshtein.Distance(queries[i], targets[j], MaxDistance);
                    }
                }
            }),
        };

        // Warm-up JIT
        foreach (var bc in benchmarkCases)
        {
            bc.Action();
        }

        Console.WriteLine("| Method | Variant | Mean (10k ops) | Time / Op | Speedup | Allocated Memory |");
        Console.WriteLine("| :--- | :--- | :---: | :---: | :---: | :---: |");

        for (int m = 0; m < benchmarkCases.Length; m += 2)
        {
            var fullCase = benchmarkCases[m];
            var earlyExitCase = benchmarkCases[m + 1];

            // Measure Full
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            long allocBeforeFull = GC.GetAllocatedBytesForCurrentThread();
            long swStartFull = Stopwatch.GetTimestamp();
            for (int it = 0; it < iterations; it++)
            {
                fullCase.Action();
            }
            long swStopFull = Stopwatch.GetTimestamp();
            long allocAfterFull = GC.GetAllocatedBytesForCurrentThread();
            double fullMs = (double)(swStopFull - swStartFull) * 1000.0 / Stopwatch.Frequency / iterations;
            long fullAlloc = (allocAfterFull - allocBeforeFull) / iterations;

            // Measure Early-Exit
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            long allocBeforeEE = GC.GetAllocatedBytesForCurrentThread();
            long swStartEE = Stopwatch.GetTimestamp();
            for (int it = 0; it < iterations; it++)
            {
                earlyExitCase.Action();
            }
            long swStopEE = Stopwatch.GetTimestamp();
            long allocAfterEE = GC.GetAllocatedBytesForCurrentThread();
            double eeMs = (double)(swStopEE - swStartEE) * 1000.0 / Stopwatch.Frequency / iterations;
            long eeAlloc = (allocAfterEE - allocBeforeEE) / iterations;

            double speedup = fullMs / eeMs;
            string methodName = fullCase.MethodName.Replace(" (Full)", "");

            double fullNsPerOp = (fullMs * 1_000_000.0) / totalOperations;
            double eeNsPerOp = (eeMs * 1_000_000.0) / totalOperations;

            Console.WriteLine($"| {methodName} | **Full** | {fullMs:F2} ms | {fullNsPerOp:F1} ns | 1.00x | {FormatBytes(fullAlloc)} |");
            Console.WriteLine($"| {methodName} | **Early-Exit** | **{eeMs:F2} ms** | **{eeNsPerOp:F1} ns** | **{speedup:F2}x** | {FormatBytes(eeAlloc)} |");
        }

        Console.WriteLine();
    }

    private static string FormatBytes(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        return $"{bytes / (1024.0 * 1024.0):F1} MB";
    }
}

