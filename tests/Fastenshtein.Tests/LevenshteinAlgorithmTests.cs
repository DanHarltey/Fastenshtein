namespace Fastenshtein.Tests
{
    using System;
    using System.Diagnostics;
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
        public void Wide_Chars_Are_Supported_Test()
        {
            Test("😄😄😄", "😄😓😄", 1);
        }

        [Fact]
        public void EmtpyStrings_Returns_Zero_Test()
        {
            Test(string.Empty, string.Empty, 0);
        }

        ////[Fact]
        ////public void Fuzzy_Test()
        ////{
        ////    var timer = Stopwatch.StartNew();

        ////    while (timer.ElapsedMilliseconds < 5000)
        ////    {
        ////        var randomWords = RandomWords.Create(10, 1000);

        ////        foreach (var word1 in randomWords)
        ////        {
        ////            foreach (var word2 in randomWords)
        ////            {
        ////                var expected = SimpleLevenshteinDistance(word1, word2);
        ////                Test(word1, word2, expected);
        ////            }
        ////        }
        ////    }
        ////}

        protected abstract int CalculateDistance(string value1, string value2);

        private void Test(string value1, string value2, int expected)
        {
            int actual = this.CalculateDistance(value1, value2);
            Assert.Equal(expected, actual);
        }

        private static int SimpleLevenshteinDistance(string value1, string value2)
        {
            if (value1.Length == 0)
            {
                return value2.Length;
            }

            if (value2.Length == 0)
            {
                return value1.Length;
            }

            int[] previousRow = new int[value2.Length + 1];
            int[] currentRow = new int[value2.Length + 1];
            int[] vtemp;

            for (int i = 0; i < previousRow.Length; i++)
            {
                previousRow[i] = i;
            }

            for (int i = 0; i < value1.Length; i++)
            {
                currentRow[0] = i + 1;

                int minv1 = currentRow[0];

                for (int j = 0; j < value2.Length; j++)
                {
                    int cost = 1;

                    if (value1[i] == value2[j])
                    {
                        cost = 0;
                    }

                    currentRow[j + 1] = Math.Min(
                        currentRow[j] + 1, // Cost of insertion
                        Math.Min(
                            previousRow[j + 1] + 1, // Cost of remove
                            previousRow[j] + cost)); // Cost of substitution

                    minv1 = Math.Min(minv1, currentRow[j + 1]);
                }

                vtemp = previousRow;
                previousRow = currentRow;
                currentRow = vtemp;
            }

            return previousRow[value2.Length];
        }
    }
}
