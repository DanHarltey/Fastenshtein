namespace SpeedTestApp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    static class Program
    {
        static void Main(string[] args)
        {
            List<ILevenshteinFactory> factoryList= Program.CreateFactories();

            // this will remove factories that do not produce correct results
            ResultsTest(factoryList);

            ILevenshteinFactory[] factories = factoryList.ToArray();

            Console.WriteLine("WarmUp Test" + Environment.NewLine);
            const int warmUpTestSize = 1000;
            SpeedTest(factories, warmUpTestSize);
            Console.WriteLine();

            Console.WriteLine("Quick Test" + Environment.NewLine);
            const int quickTestSize = 9000;
            SpeedTest(factories, quickTestSize);
            Console.WriteLine();

            Console.WriteLine("Full Test" + Environment.NewLine);
            const int fullTestSize = 23000;
            SpeedTest(factories, fullTestSize);
            Console.WriteLine();
        }

        private static List<ILevenshteinFactory> CreateFactories()
        {
            var type = typeof(ILevenshteinFactory);

            var factoryTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(p => type.IsAssignableFrom(p));
 
            List<ILevenshteinFactory> factories = new List<ILevenshteinFactory>();

            foreach (var factoryType in factoryTypes)
            {
                var factoryConstructor = factoryType.GetConstructor(Type.EmptyTypes);

                if (factoryConstructor != null)
                {
                    ILevenshteinFactory factoryInstance = (ILevenshteinFactory)factoryConstructor.Invoke(new object[0]);
                    factories.Add(factoryInstance);
                }
            }

            return factories;
        }

        private static void ResultsTest(List<ILevenshteinFactory> factories)
        {
            // get 100 random words
            string[] words = RandomWords.Create(100, 15);

            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words.Length; j++)
                {
                    int expected = 0;

                    // for all factories
                    for (int k = 0; k < factories.Count; k++)
                    {
                        var lev = factories[k].Create(words[i]);
                        int actual = lev.Distance(words[j]);

                        if (0 == k)
                        {
                            // the first factory is taken as the expected result
                            expected = actual;
                        }
                        else if (expected != actual)
                        {
                            // if the current factory does not match the current expected
                            Console.WriteLine($"Incorrect score for Levenshtein score for : {factories[k].Name} removed from further tests");
                            factories.RemoveAt(k);
                            --k;
                        }
                    }
                }
            }
        }

        private static void SpeedTest(ILevenshteinFactory[] factories, int testSize)
        {
            string[] words;

            Console.WriteLine("Normal Test");
            words = RandomWords.Create(testSize, 20);
            TestSpeed(factories, words);

            Console.WriteLine("Large Words Test");
            words = RandomWords.Create(testSize / 18, 400);
            TestSpeed(factories, words);

            Console.WriteLine("Small Words Test");
            words = RandomWords.Create(testSize * 3, 5);
            TestSpeed(factories, words);
        }

        private static void TestSpeed(ILevenshteinFactory[] factories, string[] words)
        {

            decimal[] times = new decimal[factories.Length];

            for (int i = 0; i < factories.Length; i++)
            {
                TimeSpan timeTaken = TimeSpan.MaxValue;

                // do the test 5 times and take the min value to rule out any anomalies
                for (int numberOfTimes = 0; numberOfTimes < 5; numberOfTimes++)
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

                    // min
                    timeTaken = timeTaken < stopwatch.Elapsed ? timeTaken : stopwatch.Elapsed;
                }

                times[i] = timeTaken.Ticks;
                Console.WriteLine(timeTaken + "\t" + factories[i].Name);
            }

            decimal minTime = times.Min();

            for (int i = 0; i < times.Length; i++)
            {
                decimal difference = times[i] - minTime;
                decimal percentSlower = (difference / minTime) * 100;
                percentSlower = Math.Round(percentSlower, 0);

                Console.WriteLine(percentSlower + "%\t" + factories[i].Name);
            }

            Console.WriteLine();
        }
    }
}