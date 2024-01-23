using BenchmarkDotNet.Attributes;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Fastenshtein.Benchmarking
{
    [RankColumn, DisassemblyDiagnoser(printSource: true)]
    public class ArrayBenchmark
    {
        private static readonly int[] indexes = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
        private static readonly int vectorMask;

        static ArrayBenchmark() => vectorMask = ~(Vector<int>.Count - 1);

        [Params(7)]
        ////[Params(3, 7, 8, 31, 20_000, 1_000_000)]
        public int N;

        private int[] _array;

        [GlobalSetup]
        public void SetUp()
        {
            this._array = new int[N];
        }

        ////[Benchmark()]
        ////public int[] Simple()
        ////{
        ////    for (int i = 0; i < _array.Length; i++)
        ////    {
        ////        _array[i] = i;
        ////    }

        ////    return _array;
        ////}

        ////[Benchmark(Baseline = true)]
        ////public int[] SimpleLocal()
        ////{
        ////    var tmp = this._array;

        ////    for (int i = 0; i < tmp.Length; i++)
        ////    {
        ////        tmp[i] = i;
        ////    }

        ////    return tmp;
        ////}

        ////[Benchmark]
        ////public unsafe int[] PointerUnrollMask()
        ////{
        ////    var tmp = this._array;
        ////    var i = 0;
        ////    var lastBlockIndex = tmp.Length & 0xFFFC;

        ////    fixed (int* pSource = tmp)
        ////    {
        ////        while (i < lastBlockIndex)
        ////        {
        ////            pSource[i + 0] = i;
        ////            pSource[i + 1] = i + 1;
        ////            pSource[i + 2] = i + 2;
        ////            pSource[i + 3] = i + 3;
        ////            i += 4;
        ////        }

        ////        while (i < tmp.Length)
        ////        {
        ////            pSource[i] = i;
        ////            i++;
        ////        }
        ////    }

        ////    return tmp;
        ////}

        ////[Benchmark()]
        ////public int[] SimpleLocalWithCopy()
        ////{
        ////    var tmp = this._array;

        ////    var i = Math.Min(indexes.Length, tmp.Length);

        ////    Buffer.BlockCopy(indexes, 0, tmp, 0, i * sizeof(int));

        ////    for (; i < tmp.Length; i++)
        ////    {
        ////        tmp[i] = i;
        ////    }

        ////    return tmp;
        ////}

        ////[Benchmark()]
        ////public int[] ArrayCopy()
        ////{
        ////    var indexes = ArrayBenchmark.indexes;
        ////    var tmp = this._array;

        ////    var maxCopy = Math.Min(indexes.Length, tmp.Length);

        ////    for (int i = 0; i < maxCopy; i++)
        ////    {
        ////        tmp[i] = indexes[i];
        ////    }

        ////    for (int i = maxCopy; i < tmp.Length; i++)
        ////    {
        ////        tmp[i] = i;
        ////    }

        ////    return tmp;
        ////}

        private static readonly Vector<int> additionVector = new(Vector<int>.Count);
        private static readonly Vector<int> indexesVector = new(indexes);

        ////[Benchmark(Baseline = true)]
        ////public int[] VectorPopulate()
        ////{
        ////    var tmp = _array;

        ////    var i = 0;

        ////    if (Vector.IsHardwareAccelerated)
        ////    {
        ////        var lastBlockIndex = tmp.Length - (tmp.Length % Vector<int>.Count);

        ////        var previous = indexesVector;

        ////        while (i < lastBlockIndex)
        ////        {
        ////            previous.StoreUnsafe(ref tmp[i]);
        ////            previous += additionVector;
        ////            i += Vector<int>.Count;
        ////        }
        ////    }

        ////    for (; i < tmp.Length; i++)
        ////    {
        ////        tmp[i] = i;
        ////    }

        ////    return tmp;
        ////}

        ////[Benchmark()]
        ////public int[] VectorPopulate3()
        ////{
        ////    var tmp = _array;

        ////    var i = 0;

        ////    if (Vector.IsHardwareAccelerated)
        ////    {
        ////        var lastBlockIndex = tmp.Length & vectorMask;

        ////        var previous = indexesVector;

        ////        ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);

        ////        while (i < lastBlockIndex)
        ////        {
        ////            ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
        ////            previous.StoreUnsafe(ref itemRef);
        ////            previous += additionVector;
        ////            i += Vector<int>.Count;
        ////        }
        ////    }

        ////    for (; i < tmp.Length; i++)
        ////    {
        ////        tmp[i] = i;
        ////    }

        ////    return tmp;
        ////}

        ////[Benchmark()]
        ////public int[] VectorPopulate4()
        ////{
        ////    var tmp = _array;

        ////    var i = 0;

        ////    if (Vector.IsHardwareAccelerated)
        ////    {
        ////        var lastBlockIndex = tmp.Length & vectorMask;

        ////        var previous = indexesVector;

        ////        ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);

        ////        while (i < lastBlockIndex)
        ////        {
        ////            ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
        ////            previous.StoreUnsafe(ref itemRef);
        ////            previous += additionVector;
        ////            i += Vector<int>.Count;
        ////        }
        ////    }

        ////    if (i >= 0)
        ////    {
        ////        for (; i < tmp.Length; i++)
        ////        {
        ////            tmp[i] = i;
        ////        }
        ////    }

        ////    return tmp;
        ////}

        [Benchmark()]
        public int[] VectorPopulateFinal()
        {
            var tmp = _array;
            ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
            var i = 0;

            if (Vector.IsHardwareAccelerated)
            {
                var lastBlockIndex = tmp.Length & vectorMask;

                var vector = indexesVector;

                while (i < lastBlockIndex)
                {
                    ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
                    vector.StoreUnsafe(ref itemRef);
                    vector += additionVector;
                    i += Vector<int>.Count;
                }
            }

            for (; i < tmp.Length; i++)
            {
                ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
                itemRef = i;
            }

            return tmp;
        }

        ////[Benchmark]
        ////public unsafe int[] VectorPopulate128()
        ////{
        ////    var tmp = _array;

        ////    int i = 0;

        ////    if (Vector128.IsHardwareAccelerated)
        ////    {
        ////        var lastBlockIndex = tmp.Length - (tmp.Length % Vector128<int>.Count);

        ////        var addVector = Vector128.Create(Vector128<int>.Count);
        ////        var vector = Vector128.Create(indexes);

        ////        while (i < lastBlockIndex)
        ////        {
        ////            vector.StoreUnsafe(ref tmp[i]);
        ////            vector += addVector;
        ////            i += Vector128<int>.Count;
        ////        }

        ////        for (; i < tmp.Length; i++)
        ////        {
        ////            tmp[i] = i;
        ////        }
        ////    }

        ////    return tmp;
        ////}

        ////[Benchmark]
        ////public int[] Vector256Populate()
        ////{
        ////    var tmp = _array;

        ////    int i = 0;

        ////    if (Vector256.IsHardwareAccelerated)
        ////    {
        ////        int lastBlockIndex = tmp.Length - (tmp.Length % Vector256<int>.Count);

        ////        var addVector = Vector256.Create(Vector256<int>.Count);
        ////        var vector = Vector256.Create(indexes);

        ////        while (i < lastBlockIndex)
        ////        {
        ////            vector.StoreUnsafe(ref tmp[i]);
        ////            vector += addVector;
        ////            i += Vector256<int>.Count;
        ////        }
        ////    }

        ////    for (; i < tmp.Length; i++)
        ////    {
        ////        tmp[i] = i;
        ////    }

        ////    return tmp;
        ////}

        private static readonly Vector512<int> additionVector512 = Vector512.Create(Vector512<int>.Count);
        private static readonly Vector512<int> indexesVector512 = Vector512.Create(indexes);

        [Benchmark]
        public int[] Vector512Populate()
        {
            var tmp = _array;
            ref var arrayRef = ref MemoryMarshal.GetArrayDataReference(tmp);
            var i = 0;

            if (Vector512.IsHardwareAccelerated)
            {
                var lastBlockIndex = tmp.Length - (tmp.Length % Vector512<int>.Count);

                var vector = indexesVector512;

                while (i < lastBlockIndex)
                {
                    ref var itemRef = ref Unsafe.Add(ref arrayRef, i);
                    vector.StoreUnsafe(ref itemRef);
                    vector += additionVector512;
                    i += Vector512<int>.Count;
                }
            }

            for (; i < tmp.Length; i++)
            {
                tmp[i] = i;
            }

            return tmp;
        }
    }
}
