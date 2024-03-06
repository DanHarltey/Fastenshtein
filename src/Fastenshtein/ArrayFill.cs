#if NET8_0_OR_GREATER 
using System.Numerics;
#endif
using System.Runtime.CompilerServices;

namespace Fastenshtein
{
    internal static class ArrayFill
    {
#if NET8_0_OR_GREATER 
        private static readonly int[] indexes = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16];
        private static readonly Vector<int> additionVector = new(Vector<int>.Count);
        private static readonly Vector<int> indexesVector = new(indexes);
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void PopulateCosts(int[] costs)
        {
            int i = 0;
#if NET8_0_OR_GREATER
            if (Vector.IsHardwareAccelerated)
            {
                int lastBlockIndex = costs.Length - (costs.Length % Vector<int>.Count);

                var previous = indexesVector;

                while (i < lastBlockIndex)
                {
                    previous.StoreUnsafe(ref costs[i]);
                    previous += additionVector;
                    i += Vector<int>.Count;
                }
            }
#endif
            for (; i < costs.Length;)
            {
                costs[i] = ++i;
            }
        }
    }
}