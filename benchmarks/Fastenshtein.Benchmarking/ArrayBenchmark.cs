using BenchmarkDotNet.Attributes;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

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

        [Benchmark()]
        public int[] Simple()
        {
            for (int i = 0; i < _array.Length; i++)
            {
                _array[i] = i;
            }

            return _array;
        }

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

        [Benchmark()]
        public int[] SimpleLocalWithCopy()
        {
            var tmp = this._array;

            var min = Math.Min(indexes.Length, tmp.Length);

            Buffer.BlockCopy(indexes, 0, tmp, 0, min * sizeof(int));


            for (int i = min; i < tmp.Length; i++)
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
                fixed (int* fp = tmp)
                {
                    int* p = fp;

                    for (int i = 0; i < tmp.Length; i++)
                    {
                        *p = i;
                        p++;
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
                fixed (int* fp = tmp)
                {

                    var length = tmp.Length - 1;

                    int* p = &fp[length];

                    while (length > 2)
                    {
                        *p = length--;
                        p--;
                        *p = length--;
                        p--;
                        *p = length--;
                        p--;
                        *p = length--;
                        p--;
                    }

                    while (length > 0)
                    {
                        *p = length--;
                        p--;
                    }
                }

                return tmp;
            }
        }

        [Benchmark]
        public int[] UnsafeAdd()
        {
            var tmp = this._array;

            ref var reference = ref MemoryMarshal.GetArrayDataReference(tmp);

            for (int i = 0; i < tmp.Length; i++)
            {
                ref var value = ref Unsafe.Add(ref reference, i);
                value = i;
            }
            return tmp;
        }

        private static readonly int[] indexes = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
        private static readonly Vector128<int> indexes128 = Vector128.Create(indexes);
        private static readonly Vector256<int> indexes256 = Vector256.Create(indexes);
        private static readonly Vector512<int> indexes512 = Vector512.Create(indexes);

        [Benchmark]
        public int[] See()
        {
            var tmp = this._array;
            unsafe
            {
                fixed (int* fp = tmp)
                {
                    int* p = fp;

                    var length = tmp.Length - 1;

                    var addVector = Vector128.Create(Vector128<int>.Count);
                    var previous = indexes128;

                    while (length >= Vector128<int>.Count)
                    {
                        Sse2.Store(p, previous);
                        p += Vector128<int>.Count;

                        previous = Sse2.Add(previous, addVector);
                        length -= Vector128<int>.Count;
                    }

                    for (int i = (tmp.Length - length) - 1; i < tmp.Length; i++)
                    {
                        tmp[i] = i;
                    }
                }
            }

            return tmp;
        }

        [Benchmark]
        public int[] See256()
        {
            var tmp = this._array;
            unsafe
            {
                fixed (int* fp = tmp)
                {
                    int* p = fp;

                    var length = tmp.Length - 1;

                    var addVector = Vector256.Create(Vector256<int>.Count);
                    var previous = indexes256;

                    while (length >= Vector256<int>.Count)
                    {
                        Avx.Store(p, previous);
                        p += Vector256<int>.Count;

                        previous = Avx2.Add(previous, addVector);
                        length -= Vector256<int>.Count;
                    }

                    for (int i = (tmp.Length - length) - 1; i < tmp.Length; i++)
                    {
                        tmp[i] = i;
                    }
                }
            }

            return tmp;
        }

        [Benchmark]
        public int[] SeeAny()
        {
            var tmp = _array;
            unsafe
            {
                fixed (int* fp = tmp)
                {
                    int* p = fp;

                    var length = tmp.Length - 1;

                    var addVector = new Vector<int>(Vector<int>.Count);
                    var previous = new Vector<int>(indexes);

                    while (length >= Vector<int>.Count)
                    {
                        previous.Store(p);
                        p += Vector<int>.Count;

                        previous += addVector;
                        length -= Vector<int>.Count;
                    }

                    for (int i = (tmp.Length - length) - 1; i < tmp.Length; i++)
                    {
                        tmp[i] = i;
                    }
                }
            }

            return tmp;
        }
    }
}
