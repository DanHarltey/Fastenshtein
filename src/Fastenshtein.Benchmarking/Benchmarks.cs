namespace Fastenshtein.Benchmarking
{
    public class BenchmarkSmallWordsSingleThread : BenchmarkSingleThread
    {
        protected override string[] CreateTestData() => RandomWords.Create(90, 5);
    }

    public class BenchmarkNormalWordsSingleThread : BenchmarkSingleThread
    {
        protected override string[] CreateTestData() => RandomWords.Create(60, 20);
    }

    public class BenchmarkLargeWordsSingleThread : BenchmarkSingleThread
    {
        protected override string[] CreateTestData() => RandomWords.Create(20, 400);
    }

    public class BenchmarkSmallWordsMultiThread : BenchmarkMultiThread
    {
        protected override string[] CreateTestData() => RandomWords.Create(100, 5);
    }

    public class BenchmarkNormalWordsMultiThread : BenchmarkMultiThread
    {
        protected override string[] CreateTestData() => RandomWords.Create(90, 20);
    }

    public class BenchmarkLargeWordsMultiThread : BenchmarkMultiThread
    {
        protected override string[] CreateTestData() => RandomWords.Create(50, 400);
    }
}
