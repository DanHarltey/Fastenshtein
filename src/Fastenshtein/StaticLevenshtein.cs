using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


#if NETCOREAPP
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif
namespace Fastenshtein
{
    /// <summary>
    /// Measures the difference between two strings.
    /// Uses the Levenshtein string difference algorithm.
    /// </summary>
    public partial class Levenshtein
    {
        /// <summary>
        /// Compares the two values to find the minimum Levenshtein distance. 
        /// Thread safe.
        /// </summary>
        /// <returns>Difference. 0 complete match.</returns>
        public static int Distance(string value1, string value2)
        {
#if NETCOREAPP
            if (Vector.IsHardwareAccelerated)//&& value1.Length > Vector<int>.Count)
            {
                return VectorDistance(value1, value2);
            }
#endif

            return DistanceOG(value1, value2);
        }


#if NETCOREAPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe int VectorDistance(string value1, string value2)
        {
            if (value1.Length == 0)
            {
                return value2.Length;
            }

            if (value2.Length == 0)
            {
                return value1.Length;
            }

            var diag1Array = new int[value1.Length + 1];
            var diag2Array = new int[value1.Length + 1];

            for (int counter = 1; counter <= value1.Length + value2.Length; counter++)
            {
                int startRow;

                diag2Array[0] = counter - 1;

                if (counter > value2.Length)
                {
                    startRow = counter - value2.Length;
                }
                else
                {
                    startRow = 1;
                }

                int endRow;
                if (counter > value1.Length)
                {
                    endRow = value1.Length;
                }
                else
                {
                    diag1Array[counter] = counter;
                    endRow = counter - 1;
                }

                for (int rowIndex = endRow; rowIndex >= startRow;)
                {
                    int columnIndex = counter - rowIndex;

                    ////Console.WriteLine($"{columnIndex} {rowIndex}");
                    if (rowIndex >= Vector128<int>.Count && value2.Length - columnIndex >= Vector128<int>.Count)
                    {
                        VectorDistanceRow2(diag1Array, diag2Array, value1, value2, rowIndex, columnIndex);
                        rowIndex -= Vector128<int>.Count;
                    }
                    else
                    {
                        var localCost = Math.Min(diag2Array[rowIndex], diag2Array[rowIndex - 1]);

                        if (localCost < diag1Array[rowIndex - 1])
                        {
                            diag1Array[rowIndex] = localCost + 1;
                        }
                        else
                        {
                            int cost;
                            if (value1[rowIndex - 1] != value2[columnIndex - 1])
                            {
                                cost = 1;
                            }
                            else
                            {
                                cost = 0;
                            }

                            diag1Array[rowIndex] = diag1Array[rowIndex - 1] + cost;
                        }
                        rowIndex--;
                    }
                }

                var tmpArray = diag1Array;
                diag1Array = diag2Array;
                diag2Array = tmpArray;
            }

            return diag2Array[diag2Array.Length - 1];
        }

        private readonly static int[] Reverse = [3, 2, 1, 0];

        private static unsafe Vector128<int> IntFromChar(string value, int index)
        {
            Span<int> tmp = stackalloc int[4];
            tmp[3] = value[index + 3];
            tmp[2] = value[index + 2];
            tmp[1] = value[index + 1];
            tmp[0] = value[index];

            return Vector128.Create<int>(tmp);
        }

        private static unsafe Vector128<int> IntFromCharReverse(string value, int index)
        {
            Span<int> tmp = stackalloc int[4];
            tmp[0] = value[index + 3];
            tmp[1] = value[index + 2];
            tmp[2] = value[index + 1];
            tmp[3] = value[index];

            return Vector128.Create<int>(tmp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void VectorDistanceRow2(int[] diag1Array, int[] diag2Array, string value1, string value2, int rowIndex, int columnIndex)
        {
            ref var diag1ArrayRef = ref MemoryMarshal.GetArrayDataReference(diag1Array);
            ref var diag2ArrayRef = ref MemoryMarshal.GetArrayDataReference(diag2Array);
            ////ref var reverse = ref MemoryMarshal.GetArrayDataReference(Reverse);

            ////fixed (char* sourcePtr = value1)
            ////fixed (char* targetPtr = value2)
            var sourceVector = IntFromChar(value1, rowIndex - Vector128<int>.Count);// Sse41.ConvertToVector128Int32((ushort*)sourcePtr + rowIndex - Vector128<int>.Count);
            var targetVector = IntFromCharReverse(value2, columnIndex - 1);// Sse41.ConvertToVector128Int32((ushort*)targetPtr + columnIndex - 1);
                                                                           //// targetVector = Vector128.Shuffle(targetVector, Vector128.LoadUnsafe(ref reverse));
                                                                           //// targetVector = Sse2.Shuffle(targetVector, 0x1b);

            var substitutionCostAdjustment = Sse2.CompareEqual(sourceVector, targetVector);

            var diag1Vector = Vector128.LoadUnsafe(
                ref Unsafe.Add(ref diag1ArrayRef, rowIndex - Vector128<int>.Count));

            var substitutionCost = diag1Vector + substitutionCostAdjustment;

            var deleteCost = Vector128.LoadUnsafe(
                ref Unsafe.Add(ref diag2ArrayRef, rowIndex - (Vector128<int>.Count - 1)));

            var insertCost = Vector128.LoadUnsafe(
                ref Unsafe.Add(ref diag2ArrayRef, rowIndex - Vector128<int>.Count));

            var localCost = Vector128.Min(
                Vector128.Min(insertCost, deleteCost),
                substitutionCost);

            localCost += Vector128.Create(1);

            localCost.StoreUnsafe(
                ref Unsafe.Add(ref diag1ArrayRef, rowIndex - (Vector128<int>.Count - 1)));
        }

#endif

        /// <summary>
        /// Compares the two values to find the minimum Levenshtein distance. 
        /// Thread safe.
        /// </summary>
        /// <returns>Difference. 0 complete match.</returns>
        private static int DistanceOG(string value1, string value2)
        {
            if (value2.Length == 0)
            {
                return value1.Length;
            }

            int[] costs = new int[value2.Length];

            // Add indexing for insertion to first row
            for (int i = 0; i < costs.Length;)
            {
                costs[i] = ++i;
            }

            for (int i = 0; i < value1.Length; i++)
            {
                // cost of the first index
                int cost = i;
                int previousCost = i;

                // cache value for inner loop to avoid index lookup and bonds checking, profiled this is quicker
                char value1Char = value1[i];

                for (int j = 0; j < value2.Length; j++)
                {
                    int currentCost = cost;

                    // assigning this here reduces the array reads we do, improvement of the old version
                    cost = costs[j];

                    if (value1Char != value2[j])
                    {
                        if (previousCost < currentCost)
                        {
                            currentCost = previousCost;
                        }

                        if (cost < currentCost)
                        {
                            currentCost = cost;
                        }

                        ++currentCost;
                    }

                    /* 
                     * Improvement on the older versions.
                     * Swapping the variables here results in a performance improvement for modern intel CPU’s, but I have no idea why?
                     */
                    costs[j] = currentCost;
                    previousCost = currentCost;
                }
            }

            return costs[costs.Length - 1];
        }
    }
}
