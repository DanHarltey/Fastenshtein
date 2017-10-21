namespace Fastenshtein.Benchmarking
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Exporters;
    using System.Threading.Tasks;

    [RPlotExporter]
    [CsvMeasurementsExporter]
    public abstract class BenchmarkMultiThread
    {
        protected string[] words;

        protected abstract string[] CreateTestData();

        [GlobalSetup]
        public void SetUp()
        {
            this.words = this.CreateTestData();
        }

        /*
         * To add your own Levenshtein to the benchmarking alter the below code.
         * Replace YourLevenshtein with your method.
         */
        ////[Benchmark]
        ////public void YourLevenshtein()
        ////{
        ////    Parallel.For(0, words.Length, i =>
        ////    {
        ////        for (int j = 0; j < words.Length; j++)
        ////        {
        ////            YourLevenshtein(words[i], words[j]);
        ////        }
        ////    });
        ////}

        [Benchmark(Baseline = true)]
        public void Fastenshtein()
        {
            Parallel.For(0, words.Length, i =>
            {
                var levenshtein = new global::Fastenshtein.Levenshtein(words[i]);

                for (int j = 0; j < words.Length; j++)
                {
                    levenshtein.Distance(words[j]);
                }
            });
        }

        [Benchmark]
        public void FastenshteinStatic()
        {
            Parallel.For(0, words.Length, i =>
            {
                for (int j = 0; j < words.Length; j++)
                {
                    global::Fastenshtein.Levenshtein.Distance(words[i], words[j]);
                }
            });
        }

        [Benchmark]
        public void StringSimilarity()
        {
            // I've read the source code it is thread safe
            var lev = new global::F23.StringSimilarity.Levenshtein();

            Parallel.For(0, words.Length, i =>
            {
                for (int j = 0; j < words.Length; j++)
                {
                    // why does it return a double ??
                    lev.Distance(words[i], words[j]);
                }
            });
        }

        [Benchmark]
        public void NinjaNye()
        {
            Parallel.For(0, words.Length, i =>
            {
                for (int j = 0; j < words.Length; j++)
                {
                    global::NinjaNye.SearchExtensions.Levenshtein.LevenshteinProcessor.LevenshteinDistance(words[i], words[j]);
                }
            });
        }

        [Benchmark]
        public void TNXStringManipulation()
        {
            Parallel.For(0, words.Length, i =>
            {
                for (int j = 0; j < words.Length; j++)
                {
                    global::System.LevenshteinDistanceExtensions.LevenshteinDistanceFrom(words[i], words[j]);
                }
            });
        }

        [Benchmark]
        public void MinimumEditDistance()
        {
            Parallel.For(0, words.Length, i =>
            {
                for (int j = 0; j < words.Length; j++)
                {
                    global::MinimumEditDistance.Levenshtein.CalculateDistance(words[i], words[j], 1);
                }
            });
        }
    }
}