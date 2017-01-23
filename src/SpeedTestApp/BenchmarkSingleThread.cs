namespace SpeedTestApp
{
    using BenchmarkDotNet.Attributes;

    public abstract class BenchmarkSingleThread
    {
        protected string[] words;

        protected abstract string[] CreateTestData();

        [Setup]
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
        ////    for (int i = 0; i < words.Length; i++)
        ////    {
        ////        for (int j = 0; j < words.Length; j++)
        ////        {
        ////            YourLevenshtein(words[i], words[j]);
        ////        }
        ////    }
        ////}

        [Benchmark(Baseline = true)]
        public void Fastenshtein()
        {
            for (int i = 0; i < words.Length; i++)
            {
                var levenshtein = new global::Fastenshtein.Levenshtein(words[i]);

                for (int j = 0; j < words.Length; j++)
                {
                    levenshtein.Distance(words[j]);
                }
            }
        }

        [Benchmark]
        public void FastenshteinStatic()
        {
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words.Length; j++)
                {
                    global::Fastenshtein.Levenshtein.Distance(words[i], words[j]);
                }
            }
        }

        [Benchmark]
        public void MinimumEditDistance()
        {
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words.Length; j++)
                {
                    global::MinimumEditDistance.Levenshtein.CalculateDistance(words[i], words[j], 1);
                }
            }
        }

        [Benchmark]
        public void NinjaNye()
        {
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words.Length; j++)
                {
                    global::NinjaNye.SearchExtensions.Levenshtein.LevenshteinProcessor.LevenshteinDistance(words[i], words[j]);
                }
            }
        }

        [Benchmark]
        public void StringSimilarity()
        {
            // I've read the source code it is thread safe
            var lev = new global::F23.StringSimilarity.Levenshtein();

            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words.Length; j++)
                {
                    // why does it return a double ??
                    lev.Distance(words[i], words[j]);
                }
            }
        }

        [Benchmark]
        public void TNXStringManipulation()
        {
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words.Length; j++)
                {
                    global::System.LevenshteinDistanceExtensions.LevenshteinDistanceFrom(words[i], words[j]);
                }
            }
        }
    }
}