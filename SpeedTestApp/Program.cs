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
        private const int WarmUpTestSize = 1000;
        private const int SingleThreadTestSize = 11000;
        private const int MultiThreadTestSize = 23000;

        // Load the test data once to stop memory access times affecting performance
        private static string[] LargeWords = RandomWords.Create(Program.MultiThreadTestSize / 18, 400);
        private static string[] NormalWords = RandomWords.Create(Program.MultiThreadTestSize, 20);
        private static string[] SmallWords = RandomWords.Create(Program.MultiThreadTestSize * 3, 5);

        static void Main(string[] args)
        {
            List<ILevenshteinFactory> factoryList = Program.CreateFactories();

            // this will remove factories that do not produce correct results
            Program.ResultsTest(factoryList);

            ILevenshteinFactory[] factories = factoryList.ToArray();

            // Warm up rule out JIT costs
            Console.WriteLine("WarmUp Test" + Environment.NewLine);
            SpeedTest(factories, Program.WarmUpTestSize, Program.MultiThread);
            SpeedTest(factories, Program.WarmUpTestSize, Program.SingleThread);
            Console.WriteLine();

            Console.WriteLine("Single Thread Test" + Environment.NewLine);
            Program.SpeedTest(factories, Program.SingleThreadTestSize, Program.SingleThread);
            Console.WriteLine();

            Console.WriteLine("Multi Thread Test" + Environment.NewLine);
            Program.SpeedTest(factories, Program.MultiThreadTestSize, Program.MultiThread);
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

        private static void SpeedTest(
            ILevenshteinFactory[] factories,
            int testSize,
            Action<ILevenshteinFactory, string[], int> testMethod)
        {
            Console.WriteLine("Normal Test");
            Program.TestSpeed(factories, Program.NormalWords, testSize, testMethod);

            Console.WriteLine("Large Words Test");
            Program.TestSpeed(factories, Program.LargeWords, testSize / 18, testMethod);

            Console.WriteLine("Small Words Test");
            Program.TestSpeed(factories, Program.SmallWords, testSize * 3, testMethod);
        }

        private static void TestSpeed(
            ILevenshteinFactory[] factories,
            string[] words,
            int testSize,
            Action<ILevenshteinFactory, string[], int> testMethod)
        {
            TimeSpan[] times = new TimeSpan[factories.Length];

            for (int i = 0; i < factories.Length; i++)
            {
                TimeSpan timeTaken = TimeSpan.MaxValue;

                // do the test 5 times and take the min value to rule out any anomalies
                for (int numberOfTimes = 0; numberOfTimes < 3; numberOfTimes++)
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();

                    testMethod(factories[i], words, testSize);

                    stopwatch.Stop();

                    // min
                    timeTaken = timeTaken < stopwatch.Elapsed ? timeTaken : stopwatch.Elapsed;
                }

                times[i] = timeTaken;
            }

            Program.DisplayResults(factories, times);
        }

        private static void MultiThread(ILevenshteinFactory factory, string[] words, int testSize)
        {
            Parallel.For(0, testSize, i =>
            {
                ILevenshtein levenshtein = factory.Create(words[i]);
                for (int j = 0; j < testSize; j++)
                {
                    levenshtein.Distance(words[j]);
                }
            });
        }

        private static void SingleThread(ILevenshteinFactory factory, string[] words, int testSize)
        {
            for (int i = 0; i < testSize; i++)
            {
                ILevenshtein levenshtein = factory.Create(words[i]);
                for (int j = 0; j < testSize; j++)
                {
                    levenshtein.Distance(words[j]);
                }
            }
        }

        private static void DisplayResults(ILevenshteinFactory[] factories, TimeSpan[] times)
        {
            for (int i = 0; i < times.Length; i++)
            {
                Console.WriteLine(times[i] + "\t" + factories[i].Name);
            }

            decimal minTime = times
                .Select(x => x.Ticks)
                .Min();

            for (int i = 0; i < times.Length; i++)
            {
                decimal difference = times[i].Ticks - minTime;
                decimal percentSlower = (difference / minTime) * 100;
                percentSlower = Math.Round(percentSlower, 0);

                Console.WriteLine(percentSlower + "%\t" + factories[i].Name);
            }

            Console.WriteLine();
        }
    }
}