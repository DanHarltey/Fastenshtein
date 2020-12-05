using System;

namespace Fastenshtein.Tests
{
    public static class RandomWords
    {
        private static readonly char[] Letters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public static string[] Create(int size, int maxWordSize)
        {
            string[] words = new string[size];

            // using a const seed to make sure test runs are consistent.
            Random r = new Random(69);

            for (int i = 0; i < words.Length; i++)
            {
                int wordSize = r.Next(3, maxWordSize);
                char[] word = new char[wordSize];

                for (int j = 0; j < word.Length; j++)
                {
                    // createWord
                    word[j] = Letters[r.Next(0, Letters.Length)];
                }

                words[i] = new string(word);
            }
            return words;
        }
    }
}
