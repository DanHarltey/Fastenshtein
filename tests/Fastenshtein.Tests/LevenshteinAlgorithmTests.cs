namespace Fastenshtein.Tests
{
    using Fastenshtein.Benchmarking;
    using Fastenshtein.Benchmarking.FastenshteinOld;
    using Xunit;

    public abstract class LevenshteinAlgorithmTests
    {
        [Fact]
        public void Deletion_Adds_One_Distance_Test()
        {
            Test("test", "est", 1); // deletion
        }

        [Fact]
        public void Subsitation_Adds_One_Distance_Test()
        {
            Test("test", "tett", 1); // subsitation
        }

        [Fact]
        public void Addation_Adds_One_Distance_Test()
        {
            Test("test", "testt", 1); // addation
        }

        [Fact]
        public void Transposition_Adds_Two_Distance_Test()
        {
            Test("test", "tets", 2); // transposition
        }

        [Fact]
        public void Different_Case_Adds_One_Distance_Test()
        {
            Test("test", "Test", 1); // case 
        }

        [Fact]
        public void EmtpyString_Returns_Length_Test()
        {
            Test("test", string.Empty, 4);
            Test(string.Empty, "test", 4);
        }

        [Fact]
        public void EmtpyStrings_Returns_Zero_Test()
        {
            Test(string.Empty, string.Empty, 0);
        }

        [Fact]
        public void Fuzzing_Test()
        {
            string[] testData = RandomWords.Create(100000, 20);
            int[] expected = new int[testData.Length];

            for (int i = 0; i < testData.Length; i++)
            {
                expected[i] = this.CalculateDistance(testData[0], testData[i]);
            }

            // compare against known good
            Fastenshtein_1_0_0_5 levenshteinInstance = new Fastenshtein_1_0_0_5(testData[0]);
            for (int i = 0; i < testData.Length; i++)
            {
                int actual = levenshteinInstance.DistanceFrom(testData[i]);
                Assert.Equal(expected[i], actual);
            }
        }

        protected abstract int CalculateDistance(string value1, string value2);

        private void Test(string value1, string value2, int expected)
        {
            int actual = this.CalculateDistance(value1, value2);
            Assert.Equal(expected, actual);
        }
    }
}
