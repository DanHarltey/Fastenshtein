using System.Linq;
using Xunit;

namespace Fastenshtein.Tests;

public class DiagonalStaticLevenshteinTests : LevenshteinAlgorithmTests
{
    [Fact]
    public void DevTest()
    {
        DiagonalStaticLevenshtein.Distance("xx", "xxx");
    }

    [Fact]
    public void Fuzzy()
    {
        var words = RandomWords.Create(100, 100);

        foreach (var word in words)
        {
            foreach (var word2 in words)
            {
                int result = Levenshtein.Distance(word, word2);

                int result2 = this.CalculateDistance(word, word2);
                Assert.Equal(result, result2);
            }
        }
    }

    protected override int CalculateDistance(string value1, string value2)
    {
        var expected = Levenshtein.Distance(value1, value2);

        var type = typeof(DiagonalStaticLevenshtein);
        var methods = type.GetMethods();

        foreach (var method in methods)
        {
            if (method.ReturnParameter.ParameterType == typeof(int))
            {
                var parms = method.GetParameters();
                if (parms.Length == 2 && parms.All(x => x.ParameterType == typeof(string)))
                {
                    var actual = (int)method.Invoke(null, [value1, value2]);
                    Assert.Equal(expected, actual);
                }
            }
        }

        return expected;
    }
}
