namespace Fastenshtein.Benchmarking;

public class FastenshteinBenchmark_SmallWords : FastenshteinBenchmark
{
    protected override string[] CreateTestData() => RandomWords.Create(90, 5);
}

public class FastenshteinBenchmark_NormalWords : FastenshteinBenchmark
{
    protected override string[] CreateTestData() => RandomWords.Create(60, 20);
}

public class FastenshteinBenchmark_LargeWords : FastenshteinBenchmark
{
    protected override string[] CreateTestData() => RandomWords.Create(10, 400);
}

public class CompetitiveBenchmark_SmallWords : CompetitiveBenchmark
{
    protected override string[] CreateTestData() => RandomWords.Create(90, 5);
}

public class CompetitiveBenchmark_NormalWords : CompetitiveBenchmark
{
    protected override string[] CreateTestData() => RandomWords.Create(60, 20);
}

public class CompetitiveBenchmark_LargeWords : CompetitiveBenchmark
{
    protected override string[] CreateTestData() => RandomWords.Create(20, 400);
}