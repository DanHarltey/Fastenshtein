namespace Fastenshtein.Benchmarking;

using System;

public static class RandomWords
{
    private static readonly char[] Letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    public static string[] Create(int size, int maxWordSize)
    {
        var words = new string[size];

        // using a const seed to make sure runs of the performance tests are consistent.
        var random = new Random(69);

        for (var i = 0; i < words.Length; i++)
        {
            var wordSize = random.Next(512, maxWordSize);

            words[i] = string.Create(wordSize, random, static (word, r) =>
            {
                for (var j = 0; j < word.Length; j++)
                {
                    var index = r.Next(0, Letters.Length);
                    word[j] = Letters[index];
                }
            });
        }

        return words;
    }
}
