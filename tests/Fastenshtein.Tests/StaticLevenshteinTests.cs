namespace Fastenshtein.Tests
{
    using System.Threading.Tasks;
    using Xunit;

    public class StaticLevenshteinTests : LevenshteinAlgorithmTests
    {
        ////[Fact]
        ////public void IsThreadSafe_Test()
        ////{
        ////    string[] testData = RandomWords.Create(1000000, 20);
        ////    int[] expected = new int[testData.Length];

        ////    // do it signal threaded to start
        ////    for (int i = 0; i < testData.Length; i++)
        ////    {
        ////        expected[i] = this.CalculateDistance(testData[0], testData[i]);
        ////    }

        ////    // do it multithreaded, make sure we get the same results
        ////    Parallel.For(0, testData.Length, i =>
        ////    {
        ////        int actual = this.CalculateDistance(testData[0], testData[i]);
        ////        Assert.Equal(expected[i], actual);
        ////    });
        ////}

        protected override int CalculateDistance(string value1, string value2)
        {
            return Levenshtein.Distance(value1, value2);
        }
    }
}
