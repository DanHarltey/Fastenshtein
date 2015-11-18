namespace SpeedTestApp
{
    using Fastenshtein;
    using NuGetCompetitors.DuoVia;
    using NuGetCompetitors.MinimumEditDistance;
    using NuGetCompetitors.NinjaNye;
    using NuGetCompetitors.StringCompare;
    using NuGetCompetitors.TNX;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WarmUp Test" + Environment.NewLine);
            const int warmUpTestSize = 1000;
            FullTest(warmUpTestSize);
            Console.WriteLine();

            Console.WriteLine("Quick Test" + Environment.NewLine);
            const int quickTestSize = 9000;
            FullTest(quickTestSize);
            Console.WriteLine();

            Console.WriteLine("Full Test" + Environment.NewLine);
            const int fullTestSize = 23000;
            FullTest(fullTestSize);
            Console.WriteLine();

            Console.ReadLine();
        }

        private static void FullTest(int testSize)
        {
            string[] words;

            Console.WriteLine("Normal Test");
            words = RandomWords.Create(testSize, 20);
            TestSpeed(words);

            Console.WriteLine("Large Words Test");
            words = RandomWords.Create(testSize / 18, 400);
            TestSpeed(words);

            Console.WriteLine("Small Words Test");
            words = RandomWords.Create(testSize * 3, 5);
            TestSpeed(words);
        }

        private static void TestSpeed(string[] words)
        {
            ILevenshteinFactory[] factories =
            {
                new FastenshteinFactory(),
                new FastenshteinStaticFactory(),
                // the others
                new NinjaNyeFactory(),
                new TNXFactory(),
                new StringCompareFactory(),
                new DuoViaFactory(),
                new MinimumEditDistanceFactory(),
            };

            for (int i = 0; i < factories.Length; i++)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                Parallel.For(0, words.Length, j =>
                {
                    ILevenshtein levenshtein = factories[i].Create(words[j]);
                    for (int k = 0; k < words.Length; k++)
                    {
                        levenshtein.Distance(words[k]);
                    }
                });

                stopwatch.Stop();
                Console.WriteLine(factories[i].Name + "\t\t\t" + stopwatch.Elapsed);
            }

            Console.WriteLine();
        }
    }
}