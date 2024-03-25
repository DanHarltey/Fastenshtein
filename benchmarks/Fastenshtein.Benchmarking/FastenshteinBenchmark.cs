using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fastenshtein.Benchmarking
{
    [RankColumn]
    public abstract class FastenshteinBenchmark
    {
        protected string[] words;

        protected abstract string[] CreateTestData();

        [GlobalSetup]
        public void SetUp() => this.words = this.CreateTestData();

        [Benchmark]
        public void Fastenshtein()
        {
            for (int i = 0; i < words.Length; i++)
            {
                var levenshtein = new global::Fastenshtein.Levenshtein(words[i]);

                for (int j = 0; j < words.Length; j++)
                {
                    levenshtein.DistanceFrom(words[j]);
                }
            }
        }

        [Benchmark]
        public void Fastenshtein2()
        {
            for (int i = 0; i < words.Length; i++)
            {
                var levenshtein = new global::Fastenshtein.Levenshtein(words[i]);

                for (int j = 0; j < words.Length; j++)
                {
                    levenshtein.DistanceFrom2(words[j]);
                }
            }
        }

        [Benchmark]
        public void Fastenshtein3()
        {
            for (int i = 0; i < words.Length; i++)
            {
                var levenshtein = new global::Fastenshtein.Levenshtein(words[i]);

                for (int j = 0; j < words.Length; j++)
                {
                    levenshtein.DistanceFrom3(words[j]);
                }
            }
        }

        [Benchmark]
        public void Fastenshtein4()
        {
            for (int i = 0; i < words.Length; i++)
            {
                var levenshtein = new global::Fastenshtein.Levenshtein(words[i]);

                for (int j = 0; j < words.Length; j++)
                {
                    levenshtein.DistanceFrom4(words[j]);
                }
            }
        }


        [Benchmark]
        public void Fastenshtein5()
        {
            for (int i = 0; i < words.Length; i++)
            {
                var levenshtein = new global::Fastenshtein.Levenshtein(words[i]);

                for (int j = 0; j < words.Length; j++)
                {
                    levenshtein.DistanceFrom5(words[j]);
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

        [Benchmark(Baseline = true)]
        public void Fastenshtein_1_0_0_8()
        {
            for (int i = 0; i < words.Length; i++)
            {
                var levenshtein = new global::Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_0_8(words[i]);

                for (int j = 0; j < words.Length; j++)
                {
                    levenshtein.DistanceFrom(words[j]);
                }
            }
        }

        [Benchmark]
        public void FastenshteinStatic_1_0_0_8()
        {
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = 0; j < words.Length; j++)
                {
                    global::Fastenshtein.Benchmarking.FastenshteinOld.Fastenshtein_1_0_0_8.Distance(words[i], words[j]);
                }
            }
        }
    }
}
