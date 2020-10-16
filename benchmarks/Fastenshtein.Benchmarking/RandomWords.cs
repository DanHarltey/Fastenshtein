namespace Fastenshtein.Benchmarking
{
    using System;

    public static class RandomWords
    {
        private static readonly char[] Letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public static string[] Create(int size, int maxWordSize)
        {
            string[] words = new string[size];

            var random = new Random(69);

            for (int i = 0; i < words.Length; i++)
            {
                int wordSize = random.Next(3, maxWordSize);

                words[i] = string.Create(wordSize, random, (word, r) =>
                {
                    for (int j = 0; j < word.Length; j++)
                    {
                        word[j] = Letters[r.Next(0, Letters.Length)];
                    }
                });
            }

            return words;
        }
    }
}
