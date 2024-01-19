using BenchmarkDotNet.Attributes;

namespace Fastenshtein.Benchmarking
{
    [RankColumn]
    public abstract class ArrayBenchmark
    {
        [Params(4, 25, 20_000)]
        public int N;

        protected int[] array;
        ////protected nuint[] array2;

        [GlobalSetup]
        public void SetUp()
        {
            this.array = new int[N];
            ////this.array2 = new nuint[N];
        }
        ////[Benchmark(Baseline = true)]
        ////public int[] Simple()
        ////{
        ////    for (int i = 0; i < array.Length; i++)
        ////    {
        ////        this.array[i] = i;
        ////    }

        ////    return this.array;
        ////}

        [Benchmark(Baseline = true)]
        public int[] SimpleLocal()
        {
            var tmp = this.array;

            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = i;
            }

            return tmp;
        }

        //[Benchmark]
        //public nuint[] SimpleNuintLocal()
        //{
        //    var tmp = this.array2;
        //    var len = (nuint) tmp.Length;

        //    for (nuint i = 0; i < len; i++)
        //    {
        //        tmp[i] = i;
        //    }

        //    return tmp;
        //}

        [Benchmark]
        public int[] SimpleBackward()
        {
            var tmp = this.array;

            for (int i = tmp.Length - 1; i >= 0; i--)
            {
                tmp[i] = i;
            }

            return tmp;
        }

        [Benchmark]
        public int[] UnrollBackward()
        {
            var tmp = this.array;
            int i = tmp.Length - 1;
            
            for (; i > 3; i -= 4)
            {
                tmp[i] = i;
                tmp[i - 1] = i - 1;
                tmp[i - 2] = i - 2;
                tmp[i - 3] = i - 3;
            }

            for (; i >= 0; i--)
            {
                tmp[i] = i;
            }

            return tmp;
        }
    }
}
