namespace Fastenshtein.Tests
{
    using System;
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
                int actual = levenshteinInstance.DistanceFrom(testData[i]);
                Assert.Equal(expected[i], actual);
            }
        }

        [Fact]
        public void StoredLength_Returns_The_Stored_Word_Length()
        {
            var inputValue = "I am 17 in length";
            var expected = 17;
            Levenshtein levenshteinInstance = new Levenshtein(inputValue);

            Assert.Equal(expected, levenshteinInstance.StoredLength);

            var testValue = "I am different in length";
            var distance = levenshteinInstance.DistanceFrom(testValue);

            var storedLength = levenshteinInstance.StoredLength;
            Assert.Equal(expected, storedLength);

            // StoredLength is useful for calculating percentages
            var maxLength = Math.Max(storedLength, testValue.Length);
            var charsMatching = maxLength - distance;
            var percentageScore = (charsMatching * 100) / maxLength;
            Assert.Equal(62, percentageScore);
        }

        protected override int CalculateDistance(string value1, string value2)
        {
            Levenshtein lev = new Levenshtein(value1);
            return lev.DistanceFrom(value2);
        }
    }
}
