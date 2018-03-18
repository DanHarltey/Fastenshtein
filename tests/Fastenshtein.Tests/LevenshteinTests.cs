namespace Fastenshtein.Tests
{
    using Fastenshtein.Benchmarking;
    using Xunit;

    public class LevenshteinTests : LevenshteinAlgorithmTests
    {
        [Fact]
        public void Repeated_Distance_Calls_Return_Correct_Distances()
        {
            string[] testData = RandomWords.Create(100000, 20);
            int[] expected = new int[testData.Length];

            // create an instance every time
            for (int i = 0; i < testData.Length; i++)
            {
                expected[i] = this.CalculateDistance(testData[0], testData[i]);
            }

            // reuse the same instance
            Levenshtein levenshteinInstance = new Levenshtein(testData[0]);
            for (int i = 0; i < testData.Length; i++)
            {
                int actual = levenshteinInstance.Distance(testData[i]);
                Assert.Equal(expected[i], actual);
            }
        }

        protected override int CalculateDistance(string value1, string value2)
        {
            Levenshtein lev = new Levenshtein(value1);
            return lev.Distance(value2);
        }
    }
}
