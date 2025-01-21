namespace Fastenshtein.Tests
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Threading.Tasks;
    using Xunit;
#if NET6_0_OR_GREATER
    public class StaticDevLevenshteinTests : LevenshteinAlgorithmTests
    {
        [Fact]
        public void Fuzzy()
        {
            var words = RandomWords.Create(100, 100);

            foreach (var word in words)
            {
                foreach (var word2 in words)
                {
                    int result = Fastenshtein.Levenshtein.Distance(word, word2);

                    int result2 = this.CalculateDistance(word, word2);
                    Assert.Equal(result, result2);
                }
            }
        }

        protected override int CalculateDistance(string value1, string value2)
        {
            return MessNTest.Program.DGridWithRows(value1, value2);
            //var grid = MessNTest.Program.CalculateGrid(value1, value2);

            //var text =   MessNTest.Program.DGridWithRowsWorking(value1, value2, grid);

            //var fig =
            //text.Substring(text.Length - 3, 1);

            //return int.Parse(fig);
        }
    }

    public class StaticDevLevenshteinTests2 : LevenshteinAlgorithmTests
    {
        protected override int CalculateDistance(string value1, string value2)
        {
            var text = MessNTest.Program.DGridWithRows(value1, value2);

            return text;
        }
    }
#endif
}
