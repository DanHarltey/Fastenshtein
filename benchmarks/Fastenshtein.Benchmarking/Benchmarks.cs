namespace Fastenshtein.Benchmarking
{
    public class BenchmarkSmallWordsSingleThread : FastenshteinBenchmark
    {
        protected override string[] CreateTestData() => RandomWords.Create(90, 5);
    }

    public class BenchmarkNormalWordsSingleThread : FastenshteinBenchmark
    {
        protected override string[] CreateTestData() => RandomWords.Create(60, 20);
    }

    public class BenchmarkLargeWordsSingleThread : FastenshteinBenchmark
    {
        protected override string[] CreateTestData() => RandomWords.Create(10, 400);
    }

    public class CompetitiveBenchmarkSmallWordsSingleThread : CompetitiveSingleThreadBenchmark
    {
        protected override string[] CreateTestData() => RandomWords.Create(90, 5);
    }

    public class CompetitiveBenchmarkNormalWordsSingleThread : CompetitiveSingleThreadBenchmark
    {
        protected override string[] CreateTestData() => RandomWords.Create(60, 20);
    }

    public class CompetitiveBenchmarkLargeWordsSingleThread : CompetitiveSingleThreadBenchmark
    {
        protected override string[] CreateTestData() => RandomWords.Create(20, 400);
    }

    public class CompetitiveBenchmarkSmallWordsMultiThread : CompetitiveMultiThreadBenchmark
    {
        protected override string[] CreateTestData() => RandomWords.Create(100, 5);
    }

    public class CompetitiveBenchmarkNormalWordsMultiThread : CompetitiveMultiThreadBenchmark
    {
        protected override string[] CreateTestData() => RandomWords.Create(90, 20);
    }

    public class CompetitiveBenchmarkLargeWordsMultiThread : CompetitiveMultiThreadBenchmark
    {
        protected override string[] CreateTestData() => RandomWords.Create(50, 400);
    }
}
