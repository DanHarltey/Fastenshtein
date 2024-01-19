using BenchmarkDotNet.Attributes;

namespace Fastenshtein.Benchmarking
{
    [RankColumn]
    public class ArrayBenchmark
    {
        [Params(4, 25, 20_000)]
        public int N;

        private int[] _array;
        ////protected nuint[] array2;

        [GlobalSetup]
        public void SetUp()
        {
            this._array = new int[N];
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
            var tmp = this._array;

            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = i;
            }

            return tmp;
        }

        [Benchmark]
        public int[] SimpleUnroll()
        {
            var tmp = this._array;

            if (tmp.Length > 4)
            {
                var i = 4;
                for (; i < tmp.Length; i += 4)
                {
                    tmp[i - 3] = i - 3;
                    tmp[i - 2] = i - 2;
                    tmp[i - 1] = i - 1;
                    tmp[i] = i;
                }

                i -= 4;
                for (; i < tmp.Length; i++)
                {
                    tmp[i] = i;
                }
            }
            else
            {
                for (int i = 0; i < tmp.Length; i++)
                {
                    tmp[i] = i;
                }
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
            var tmp = this._array;

            for (int i = tmp.Length - 1; i >= 0; i--)
            {
                tmp[i] = i;
            }

            return tmp;
        }

        [Benchmark]
        public int[] UnrollBackward()
        {
            var tmp = this._array;
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

        [Benchmark]
        public int[] PointerSimple()
        {
            var tmp = this._array;
            unsafe
            {
                // Must pin object on heap so that it doesn't move while using interior pointers.
                fixed (int* p = tmp)
                {
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        p[i] = i;
                    }

                }
            }

            return tmp;
        }

        [Benchmark]
        public int[] PointerUnroll()
        {
            var tmp = this._array;
            unsafe
            {
                // Must pin object on heap so that it doesn't move while using interior pointers.
                fixed (int* p = tmp)
                {
                    if (tmp.Length > 4)
                    {
                        var i = 4;
                        for (; i < tmp.Length; i += 4)
                        {
                            p[i - 3] = i - 3;
                            p[i - 2] = i - 2;
                            p[i - 1] = i - 1;
                            p[i] = i;

                        }

                        i -= 4;
                        for (; i < tmp.Length; i++)
                        {
                            p[i] = i;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < tmp.Length; i++)
                        {
                            p[i] = i;
                        }
                    }

                    return tmp;
                }
            }
        }
    }
}
