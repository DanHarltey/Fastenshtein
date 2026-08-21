#if NET7_0_OR_GREATER

namespace Fastenshtein.Tests;

using System;
using Xunit;

public class LevenshteinValueTests : LevenshteinAlgorithmTests
{
    [Fact]
    public void Throws_When_Buffer_Too_Small()
    {
        var input = "four";

        // same size
        new LevenshteinValue<char>(input, new int[4]);

        // smaller
        Assert.Throws<ArgumentException>(() => new LevenshteinValue<char>(input, new int[3]));
    }

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
        var levenshteinInstance = new LevenshteinValue<char>(testData[0].AsSpan());
        for (int i = 0; i < testData.Length; i++)
        {
            int actual = levenshteinInstance.DistanceFrom(testData[i].AsSpan());
            Assert.Equal(expected[i], actual);
        }
    }

    [Fact]
    public void Repeated_Distance_Calls_Return_Correct_Distances_With_Buffer()
    {
        Span<int> buffer = stackalloc int[400];
        string[] testData = RandomWords.Create(100000, 20);
        int[] expected = new int[testData.Length];

        // create an instance every time
        for (int i = 0; i < testData.Length; i++)
        {
            var lev = new LevenshteinValue<char>(testData[0], buffer);
            expected[i] = lev.DistanceFrom(testData[i].AsSpan());
        }

        // reuse the same instance
        var levenshteinInstance = new LevenshteinValue<char>(testData[0].AsSpan(), buffer);
        for (int i = 0; i < testData.Length; i++)
        {
            int actual = levenshteinInstance.DistanceFrom(testData[i].AsSpan());
            Assert.Equal(expected[i], actual);
        }

        // check results against anther method
        for (int i = 0; i < testData.Length; i++)
        {
            int actual = Levenshtein.Distance(testData[0], testData[i]);
            Assert.Equal(expected[i], actual);
        }
    }

    [Fact]
    public void StoredLength_Returns_The_Stored_Word_Length()
    {
        var inputValue = "I am 17 in length";
        var expected = 17;
        var levenshteinInstance = new LevenshteinValue<char>(inputValue);

        Assert.Equal(expected, levenshteinInstance.StoredLength);

        var testValue = "I am different in length";
        var distance = levenshteinInstance.DistanceFrom<char>(testValue);

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
        var lev = new LevenshteinValue<char>(value1);
        return lev.DistanceFrom(value2.AsSpan());
    }
}
#endif