#if NET9_0_OR_GREATER
using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Fastenshtein;

public static class DiagonalStaticLevenshtein
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int DistanceSSE9(string xWord, string yWord)
    {
        if (xWord.Length == 0)
        {
            return yWord.Length;
        }

        var costLen = xWord.Length + 1;
        var costArray = new int[costLen * 2];
        ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
        ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
        for (int i = 0; i < costLen; i++)
        {
            Unsafe.Add(ref previousRef, i) = i;
            Unsafe.Add(ref currentRef, i) = i;
        }

        ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
        ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());

        int startX = 0;
        var endX = 0;
        var max = xWord.Length + yWord.Length;

        for (int i = 1; i < max; ++i)
        {
            previousRef = i;

            if (i <= xWord.Length)
            {
                startX = i;
            }

            if (i > yWord.Length)
            {
                ++endX;
            }

            var x = startX;
            ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

            if (Vector256.IsHardwareAccelerated)
            {
                var sseSize = (startX - endX) / (Vector256<int>.Count);

                for (int j = 0; j < sseSize; j++)
                {
                    ref var xShort = ref Unsafe.As<char, ushort>(ref xWordRef);
                    xShort = ref Unsafe.Add(ref xShort, x - Vector128<ushort>.Count);
                    var xVector = Vector128.LoadUnsafe(ref xShort);

                    ref var yShort = ref Unsafe.As<char, ushort>(ref y);
                    var yVector = Vector128.LoadUnsafe(ref yShort);

                    yVector =
                        // Sse2.Shuffle(yVector, 0x1b);
                        Vector128.Shuffle(yVector, Vector128.Create(7, 6, 5, 4, 3, 2, 1, (ushort)0));

                    var substitutionCostAdjustment128 = Vector128.Equals(xVector, yVector);

                    var val1 = substitutionCostAdjustment128
                        .ToVector256Unsafe()
                        .AsInt16();

                    var substitutionCostAdjustment = Vector256.WidenLower(val1);

                    var currentVector = Vector256.LoadUnsafe(
                        ref Unsafe.Add(ref currentRef, x - Vector256<int>.Count));

                    var substitutionCost = currentVector + substitutionCostAdjustment;

                    var deleteCost = Vector256.LoadUnsafe(
                        ref Unsafe.Add(ref previousRef, x - (Vector256<int>.Count - 1)));

                    var insertCost = Vector256.LoadUnsafe(
                        ref Unsafe.Add(ref previousRef, x - Vector256<int>.Count));

                    var localCost = Vector256.Min(
                        Vector256.Min(insertCost, deleteCost),
                        substitutionCost);

                    localCost += Vector256<int>.One;

                    localCost.StoreUnsafe(
                        ref Unsafe.Add(ref currentRef, x - (Vector256<int>.Count - 1)));

                    x -= Vector256<int>.Count;
                    y = ref Unsafe.Add(ref y, Vector256<int>.Count);
                }
            }

            for (; x > endX; x--)
            {
                var replacementCost = Unsafe.Add(ref currentRef, x - 1);

                if (Unsafe.Add(ref xWordRef, x - 1) != y)
                {
                    var deletionCost = Unsafe.Add(ref previousRef, x - 1);
                    if (deletionCost < replacementCost)
                    {
                        replacementCost = deletionCost;
                    }

                    var insertionCost = Unsafe.Add(ref previousRef, x);
                    if (insertionCost < replacementCost)
                    {
                        replacementCost = insertionCost;
                    }

                    ++replacementCost;
                }

                Unsafe.Add(ref currentRef, x) = replacementCost;
                y = ref Unsafe.Add(ref y, 1);
            }

            ref var tmp = ref previousRef;
            previousRef = ref currentRef;
            currentRef = ref tmp;
        }

        return Unsafe.Add(ref previousRef, xWord.Length);
    }

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceSSE8(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var costLen = xWord.Length + 1;
    ////    ////RuntimeHelpers.GetUninitializedObject()
    ////    // _mm_cmpeq_epi32
    ////    var costArray = new int[costLen * 2];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
    ////    ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
    ////    for (int i = 0; i < costLen; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }
    ////    ////Unsafe.CopyBlockUnaligned()

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
    ////    ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    var endX = 0;
    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        previousRef = i;

    ////        if (i <= xWord.Length)
    ////        {
    ////            startX = i;
    ////        }

    ////        if (i > yWord.Length)
    ////        {
    ////            ++endX;
    ////        }

    ////        ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

    ////        var sseSize = (startX - endX) / (Vector256<int>.Count * 2);

    ////        var x = startX;
    ////        for (int j = 0; j < sseSize; j++)
    ////        {
    ////            DistanceSSERow8(
    ////                ref previousRef,
    ////                ref currentRef,
    ////                ref xWordRef,
    ////                ref y,
    ////                x);

    ////            x -= (Vector256<int>.Count * 2);
    ////            y = ref Unsafe.Add(ref y, (Vector256<int>.Count * 2));
    ////        }

    ////        for (; x > endX; x--)
    ////        {
    ////            var replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (Unsafe.Add(ref xWordRef, x - 1) != y)
    ////            {
    ////                var deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                var insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y = ref Unsafe.Add(ref y, 1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////private static void DistanceSSERow8(
    ////    ref int previousRef,
    ////    ref int currentRef,
    ////    ref char xString,
    ////    ref char yString,
    ////    int x)
    ////{
    ////    ref var xShort = ref Unsafe.As<char, ushort>(ref xString);
    ////    xShort = ref Unsafe.Add(ref xShort, x - Vector256<ushort>.Count);
    ////    var xVector = Vector256.LoadUnsafe(ref xShort);

    ////    ref var yShort = ref Unsafe.As<char, ushort>(ref yString);
    ////    var yVector = Vector256.LoadUnsafe(ref yShort);

    ////    yVector =
    ////        // Sse2.Shuffle(yVector, 0x1b);
    ////        Vector256.Shuffle(
    ////            yVector, 
    ////            Vector256.Create(15, 14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, (ushort)0));

    ////    var substitutionCostAdjustment = Vector256.Equals(xVector, yVector);
    ////    //var xUpperVector = Vector128.WidenUpper(xVector).AsInt32();
    ////    //var yLowerVector = Vector128.WidenLower(yVector).AsInt32();

    ////    var llllllll = substitutionCostAdjustment.AsInt16();
    ////    var tttttt = Vector256.WidenUpper(llllllll);
    ////    DoStuff7(ref previousRef, ref currentRef, x, tttttt);
    ////    x -= Vector256<int>.Count;

    ////    //var xLowerVector = Vector128.WidenLower(xVector).AsInt32();
    ////    //var yUpperVector = Vector128.WidenUpper(yVector).AsInt32();

    ////    DoStuff7(ref previousRef, ref currentRef, x, Vector256.WidenLower(llllllll));
    ////}

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int DistanceSSE7(string xWord, string yWord)
    {
        if (xWord.Length == 0)
        {
            return yWord.Length;
        }

        var costLen = xWord.Length + 1;
        ////RuntimeHelpers.GetUninitializedObject()
        // _mm_cmpeq_epi32
        var costArray = new int[costLen * 2];
        ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
        ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
        for (int i = 0; i < costLen; i++)
        {
            Unsafe.Add(ref previousRef, i) = i;
            Unsafe.Add(ref currentRef, i) = i;
        }
        ////Unsafe.CopyBlockUnaligned()

        int startX = 0;
        var max = xWord.Length + yWord.Length;

        ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
        ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
        var endX = 0;
        for (int i = 1; i < max; ++i)
        {
            previousRef = i;

            if (i <= xWord.Length)
            {
                startX = i;
            }

            if (i > yWord.Length)
            {
                ++endX;
            }

            ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

            var sseSize = (startX - endX) / (Vector256<int>.Count);

            var x = startX;
            for (int j = 0; j < sseSize; j++)
            {
                DistanceSSERow7(
                    ref previousRef,
                    ref currentRef,
                    ref xWordRef,
                    ref y,
                    x);

                x -= (Vector256<int>.Count);
                y = ref Unsafe.Add(ref y, (Vector256<int>.Count));
            }

            for (; x > endX; x--)
            {
                var replacementCost = Unsafe.Add(ref currentRef, x - 1);

                if (Unsafe.Add(ref xWordRef, x - 1) != y)
                {
                    var deletionCost = Unsafe.Add(ref previousRef, x - 1);
                    if (deletionCost < replacementCost)
                    {
                        replacementCost = deletionCost;
                    }

                    var insertionCost = Unsafe.Add(ref previousRef, x);
                    if (insertionCost < replacementCost)
                    {
                        replacementCost = insertionCost;
                    }

                    ++replacementCost;
                }

                Unsafe.Add(ref currentRef, x) = replacementCost;
                y = ref Unsafe.Add(ref y, 1);
            }

            ref var tmp = ref previousRef;
            previousRef = ref currentRef;
            currentRef = ref tmp;
        }

        return Unsafe.Add(ref previousRef, xWord.Length);
    }

    private static void DistanceSSERow7(
        ref int previousRef,
        ref int currentRef,
        ref char xString,
        ref char yString,
        int x)
    {
        ref var xShort = ref Unsafe.As<char, ushort>(ref xString);
        xShort = ref Unsafe.Add(ref xShort, x - Vector128<ushort>.Count);
        var xVector = Vector128.LoadUnsafe(ref xShort);

        ref var yShort = ref Unsafe.As<char, ushort>(ref yString);
        var yVector = Vector128.LoadUnsafe(ref yShort);

        yVector =
            // Sse2.Shuffle(yVector, 0x1b);
            Vector128.Shuffle(yVector, Vector128.Create(7, 6, 5, 4, 3, 2, 1, (ushort)0));

        var substitutionCostAdjustment = Vector128.Equals(xVector, yVector);
        //var xUpperVector = Vector128.WidenUpper(xVector).AsInt32();
        //var yLowerVector = Vector128.WidenLower(yVector).AsInt32();

        //var val1 =
        //    Vector128.Shuffle(substitutionCostAdjustment, Vector128.Create(4, 4, 5, 5, 6, 6, 7, (short)7))
        //    .AsInt32();


        var val1 = substitutionCostAdjustment
            .ToVector256Unsafe()
            .AsInt16();

        var val2 = Vector256.WidenLower(val1);
        //Unsafe.As()
        DoStuff7(ref previousRef, ref currentRef, x, val2); //// tttttt.AsInt32());
        //x -= Vector128<int>.Count;

        ////var xLowerVector = Vector128.WidenLower(xVector).AsInt32();
        ////var yUpperVector = Vector128.WidenUpper(yVector).AsInt32();

        //var val3 =
        //    Vector128.Shuffle(substitutionCostAdjustment, Vector128.Create(0, 0, 1, 1, 2, 2, 3, (short)3))
        //    .AsInt32();
        //DoStuff5(ref previousRef, ref currentRef, x, val3);
    }

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceSSE6(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var costLen = xWord.Length + 1;
    ////    ////RuntimeHelpers.GetUninitializedObject()
    ////    // _mm_cmpeq_epi32
    ////    var costArray = new int[costLen * 2];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
    ////    ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
    ////    for (int i = 0; i < costLen; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }
    ////    ////Unsafe.CopyBlockUnaligned()

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
    ////    ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    var endX = 0;
    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        previousRef = i;

    ////        if (i <= xWord.Length)
    ////        {
    ////            startX = i;
    ////        }

    ////        if (i > yWord.Length)
    ////        {
    ////            ++endX;
    ////        }

    ////        ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

    ////        var sseSize = (startX - endX) / (Vector128<int>.Count * 2);

    ////        var x = startX;
    ////        for (int j = 0; j < sseSize; j++)
    ////        {
    ////            DistanceSSERow6(
    ////                ref previousRef,
    ////                ref currentRef,
    ////                ref xWordRef,
    ////                ref y,
    ////                x);

    ////            x -= (Vector128<int>.Count * 2);
    ////            y = ref Unsafe.Add(ref y, (Vector128<int>.Count * 2));
    ////        }

    ////        for (; x > endX; x--)
    ////        {
    ////            var replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (Unsafe.Add(ref xWordRef, x - 1) != y)
    ////            {
    ////                var deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                var insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y = ref Unsafe.Add(ref y, 1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////private static void DistanceSSERow6(
    ////    ref int previousRef,
    ////    ref int currentRef,
    ////    ref char xString,
    ////    ref char yString,
    ////    int x)
    ////{
    ////    ref var xShort = ref Unsafe.As<char, short>(ref xString);
    ////    xShort = ref Unsafe.Add(ref xShort, x - Vector128<ushort>.Count);
    ////    var xVector = Vector128.LoadUnsafe(ref xShort);

    ////    ref var yShort = ref Unsafe.As<char, short>(ref yString);
    ////    var yVector = Vector128.LoadUnsafe(ref yShort);

    ////    yVector =
    ////        // Sse2.Shuffle(yVector, 0x1b);
    ////        Vector128.Shuffle(yVector, Vector128.Create(7, 6, 5, 4, 3, 2, 1, (short)0));

    ////    var substitutionCostAdjustment = Vector128.Equals(xVector, yVector);
    ////    //var xUpperVector = Vector128.WidenUpper(xVector).AsInt32();
    ////    //var yLowerVector = Vector128.WidenLower(yVector).AsInt32();

    ////    var val1 =
    ////        Vector128.Shuffle(substitutionCostAdjustment, Vector128.Create(4, 4, 5, 5, 6, 6, 7, (short)7))
    ////        .AsInt32();
    ////    ////var val2 = Vector128.WidenUpper(substitutionCostAdjustment);
    ////    //Unsafe.As()
    ////    DoStuff5(ref previousRef, ref currentRef, x, val1); //// tttttt.AsInt32());
    ////    x -= Vector128<int>.Count;

    ////    //var xLowerVector = Vector128.WidenLower(xVector).AsInt32();
    ////    //var yUpperVector = Vector128.WidenUpper(yVector).AsInt32();

    ////    var val3 =
    ////        Vector128.Shuffle(substitutionCostAdjustment, Vector128.Create(0, 0, 1, 1, 2, 2, 3, (short)3))
    ////        .AsInt32();
    ////    DoStuff5(ref previousRef, ref currentRef, x, val3);
    ////}

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceSSE5(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var costLen = xWord.Length + 1;
    ////    ////RuntimeHelpers.GetUninitializedObject()
    ////    // _mm_cmpeq_epi32
    ////    var costArray = new int[costLen * 2];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
    ////    ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
    ////    for (int i = 0; i < costLen; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }
    ////    ////Unsafe.CopyBlockUnaligned()

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
    ////    ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    var endX = 0;
    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        previousRef = i;

    ////        if (i <= xWord.Length)
    ////        {
    ////            startX = i;
    ////        }

    ////        if (i > yWord.Length)
    ////        {
    ////            ++endX;
    ////        }

    ////        ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

    ////        var sseSize = (startX - endX) / (Vector128<int>.Count * 2);

    ////        var x = startX;
    ////        for (int j = 0; j < sseSize; j++)
    ////        {
    ////            DistanceSSERow5(
    ////                ref previousRef,
    ////                ref currentRef,
    ////                ref xWordRef,
    ////                ref y,
    ////                x);

    ////            x -= (Vector128<int>.Count * 2);
    ////            y = ref Unsafe.Add(ref y, (Vector128<int>.Count * 2));
    ////        }

    ////        for (; x > endX; x--)
    ////        {
    ////            var replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (Unsafe.Add(ref xWordRef, x - 1) != y)
    ////            {
    ////                var deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                var insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y = ref Unsafe.Add(ref y, 1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////private static void DistanceSSERow5(
    ////    ref int previousRef,
    ////    ref int currentRef,
    ////    ref char xString,
    ////    ref char yString,
    ////    int x)
    ////{
    ////    ref var xShort = ref Unsafe.As<char, ushort>(ref xString);
    ////    xShort = ref Unsafe.Add(ref xShort, x - Vector128<ushort>.Count);
    ////    var xVector = Vector128.LoadUnsafe(ref xShort);

    ////    ref var yShort = ref Unsafe.As<char, ushort>(ref yString);
    ////    var yVector = Vector128.LoadUnsafe(ref yShort);

    ////    yVector =
    ////        // Sse2.Shuffle(yVector, 0x1b);
    ////        Vector128.Shuffle(yVector, Vector128.Create(7, 6, 5, 4, 3, 2, 1, (ushort)0));

    ////    var substitutionCostAdjustment = Vector128.Equals(xVector, yVector);
    ////    //var xUpperVector = Vector128.WidenUpper(xVector).AsInt32();
    ////    //var yLowerVector = Vector128.WidenLower(yVector).AsInt32();

    ////    var llllllll = substitutionCostAdjustment.AsInt16();
    ////    var tttttt = Vector128.WidenUpper(llllllll);
    ////    DoStuff5(ref previousRef, ref currentRef, x, tttttt);
    ////    x -= Vector128<int>.Count;

    ////    //var xLowerVector = Vector128.WidenLower(xVector).AsInt32();
    ////    //var yUpperVector = Vector128.WidenUpper(yVector).AsInt32();

    ////    DoStuff5(ref previousRef, ref currentRef, x, Vector128.WidenLower(llllllll));
    ////}

    private static void DoStuff7(
      ref int previousRef,
      ref int currentRef,
      int x,
      Vector256<int> substitutionCostAdjustment)
    {
        var currentVector = Vector256.LoadUnsafe(
            ref Unsafe.Add(ref currentRef, x - Vector256<int>.Count));

        var substitutionCost = currentVector + substitutionCostAdjustment;

        var deleteCost = Vector256.LoadUnsafe(
            ref Unsafe.Add(ref previousRef, x - (Vector256<int>.Count - 1)));

        var insertCost = Vector256.LoadUnsafe(
            ref Unsafe.Add(ref previousRef, x - Vector256<int>.Count));

        var localCost = Vector256.Min(
            Vector256.Min(insertCost, deleteCost),
            substitutionCost);

        localCost += Vector256<int>.One;

        localCost.StoreUnsafe(
            ref Unsafe.Add(ref currentRef, x - (Vector256<int>.Count - 1)));
    }

    ////private static void DoStuff5(
    ////    ref int previousRef,
    ////    ref int currentRef,
    ////    int x,
    ////    Vector128<int> substitutionCostAdjustment)
    ////{
    ////    var currentVector = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref currentRef, x - Vector128<int>.Count));

    ////    var substitutionCost = currentVector + substitutionCostAdjustment;

    ////    var deleteCost = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref previousRef, x - (Vector128<int>.Count - 1)));

    ////    var insertCost = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref previousRef, x - Vector128<int>.Count));

    ////    var localCost = Vector128.Min(
    ////        Vector128.Min(insertCost, deleteCost),
    ////        substitutionCost);

    ////    localCost += Vector128<int>.One;

    ////    localCost.StoreUnsafe(
    ////        ref Unsafe.Add(ref currentRef, x - (Vector128<int>.Count - 1)));
    ////}

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceSSE4(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var costLen = xWord.Length + 1;
    ////    ////RuntimeHelpers.GetUninitializedObject()
    ////    // _mm_cmpeq_epi32
    ////    var costArray = new int[costLen * 2];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
    ////    ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
    ////    for (int i = 0; i < costLen; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }
    ////    ////Unsafe.CopyBlockUnaligned()

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
    ////    ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    var endX = 0;
    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        previousRef = i;

    ////        if (i <= xWord.Length)
    ////        {
    ////            startX = i;
    ////        }

    ////        if (i > yWord.Length)
    ////        {
    ////            ++endX;
    ////        }

    ////        ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

    ////        var sseSize = (startX - endX) / (Vector128<int>.Count * 2);

    ////        var x = startX;
    ////        for (int j = 0; j < sseSize; j++)
    ////        {
    ////            DistanceSSERow4(
    ////                ref previousRef,
    ////                ref currentRef,
    ////                ref xWordRef,
    ////                ref y,
    ////                x,
    ////                sseSize);

    ////            x -= (Vector128<int>.Count * 2);
    ////            y = ref Unsafe.Add(ref y, (Vector128<int>.Count * 2));
    ////        }

    ////        for (; x > endX; x--)
    ////        {
    ////            var replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (Unsafe.Add(ref xWordRef, x - 1) != y)
    ////            {
    ////                var deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                var insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y = ref Unsafe.Add(ref y, 1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////private static void DistanceSSERow4(
    ////    ref int previousRef,
    ////    ref int currentRef,
    ////    ref char xString,
    ////    ref char yString,
    ////    int x,
    ////    int sseSize)
    ////{
    ////    ref var xShort = ref Unsafe.As<char, ushort>(ref xString);
    ////    xShort = ref Unsafe.Add(ref xShort, x - Vector128<ushort>.Count);
    ////    var xVector = Vector128.LoadUnsafe(ref xShort);

    ////    ref var yShort = ref Unsafe.As<char, ushort>(ref yString);
    ////    var yVector = Vector128.LoadUnsafe(ref yShort);

    ////    var xUpperVector = Vector128.WidenUpper(xVector).AsInt32();
    ////    var yLowerVector = Vector128.WidenLower(yVector).AsInt32();

    ////    DoStuff4(ref previousRef, ref currentRef, x, xUpperVector, yLowerVector);

    ////    x -= Vector128<int>.Count;

    ////    var xLowerVector = Vector128.WidenLower(xVector).AsInt32();
    ////    var yUpperVector = Vector128.WidenUpper(yVector).AsInt32();
    ////    DoStuff4(ref previousRef, ref currentRef, x, xLowerVector, yUpperVector);
    ////}

    ////private static void DoStuff4(
    ////    ref int previousRef,
    ////    ref int currentRef,
    ////    int x,
    ////    Vector128<int> xVector,
    ////    Vector128<int> yVector)
    ////{
    ////    yVector =
    ////        // Sse2.Shuffle(yVector, 0x1b);
    ////        Vector128.Shuffle(yVector, Vector128.Create(3, 2, 1, 0));

    ////    var substitutionCostAdjustment = Vector128.Equals(xVector, yVector);

    ////    var currentVector = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref currentRef, x - Vector128<int>.Count));

    ////    var substitutionCost = currentVector + substitutionCostAdjustment;

    ////    var deleteCost = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref previousRef, x - (Vector128<int>.Count - 1)));

    ////    var insertCost = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref previousRef, x - Vector128<int>.Count));

    ////    var localCost = Vector128.Min(
    ////        Vector128.Min(insertCost, deleteCost),
    ////        substitutionCost);

    ////    localCost += Vector128<int>.One;

    ////    localCost.StoreUnsafe(
    ////        ref Unsafe.Add(ref currentRef, x - (Vector128<int>.Count - 1)));
    ////}

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceSSE3(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var costLen = xWord.Length + 1;
    ////    ////RuntimeHelpers.GetUninitializedObject()
    ////    // _mm_cmpeq_epi32
    ////    var costArray = new int[costLen * 2];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
    ////    ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
    ////    for (int i = 0; i < costLen; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }
    ////    ////Unsafe.CopyBlockUnaligned()

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
    ////    ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    var endX = 0;
    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        // this isn't really needed after (i > yLen) but its better than a branch?
    ////        previousRef = i;

    ////        //startX += i <= xWord.Length ? 1 : 0;
    ////        if (i <= xWord.Length)
    ////        {
    ////            //++startX;
    ////            startX = i;
    ////        }

    ////        if (i > yWord.Length)
    ////        {
    ////            ++endX;// = i - yWord.Length;
    ////        }

    ////        ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

    ////        var sseSize = (startX - endX) / (Vector128<int>.Count);

    ////        var x = startX;
    ////        for (int j = 0; j < sseSize; j++)
    ////        {
    ////            DistanceSSERow3(
    ////                ref previousRef,
    ////                ref currentRef,
    ////                ref xWordRef,
    ////                ref y,
    ////                x,
    ////                sseSize);

    ////            x -= Vector128<int>.Count;
    ////            y = ref Unsafe.Add(ref y, Vector128<int>.Count);
    ////        }

    ////        for (; x > endX; x--)
    ////        {
    ////            var replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (Unsafe.Add(ref xWordRef, x - 1) != y)
    ////            {
    ////                var deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                var insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y = ref Unsafe.Add(ref y, 1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////[MethodImpl(MethodImplOptions.AggressiveInlining)]
    ////private static void DistanceSSERow3(
    ////    ref int previousRef,
    ////    ref int currentRef,
    ////    ref char xString,
    ////    ref char yString,
    ////    int x,
    ////    int sseSize)
    ////{
    ////    ref var xShort = ref Unsafe.As<char, ushort>(ref xString);
    ////    xShort = ref Unsafe.Add(ref xShort, x - Vector128<int>.Count);
    ////    //var xShortSpan = MemoryMarshal.CreateSpan(ref xShort, 8);
    ////    var xVector = Vector128.LoadUnsafe(ref xShort);
    ////    var xLowerVector = Vector128.WidenLower(xVector).AsInt32();

    ////    // Vector WidenLower ✓
    ////    ref var yShort = ref Unsafe.As<char, ushort>(ref yString);
    ////    ////yShort = ref Unsafe.Add(ref shortValue, index);
    ////    //var yShortSpan = MemoryMarshal.CreateSpan(ref yShort, 8);
    ////    //var yVector = Vector128.Create<ushort>(yShortSpan);
    ////    var yVector = Vector128.LoadUnsafe(ref yShort);
    ////    var yLowerVector2 = Vector128.WidenLower(yVector).AsInt32();
    ////    // var yLowerVector = yLowerVector2;
    ////    // Vector Shuffle ✘
    ////    var yLowerVector =
    ////        Vector128.Shuffle(yLowerVector2, Vector128.Create(3, 2, 1, 0));
    ////    ////Vector128.Shuffle ✓
    ////    ////Vector128.Shuffle
    ////    //Vector128.Negate(yLowerVector2).AsInt32();
    ////    //Sse2.Shuffle(yLowerVector2, 0x1b).AsInt32();

    ////    // var sourceVector = IntFromChar3(ref xString, x - Vector128<int>.Count);// Sse41.ConvertToVector128Int32((ushort*)sourcePtr + rowIndex - Vector128<int>.Count);
    ////    ////var targetVector = IntFromCharReverse2(ref yString, y - 1);// Sse41.ConvertToVector128Int32((ushort*)targetPtr + columnIndex - 1);
    ////    //// targetVector = Vector128.Shuffle(targetVector, Vector128.LoadUnsafe(ref reverse));
    ////    //// targetVector = Sse2.Shuffle(targetVector, 0x1b);

    ////    // Vector Equals ✓
    ////    var substitutionCostAdjustment = Sse2.CompareEqual(xLowerVector, yLowerVector);

    ////    var currentVector = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref currentRef, x - Vector128<int>.Count));

    ////    // Vector + ✓
    ////    var substitutionCost = currentVector + substitutionCostAdjustment;

    ////    var deleteCost = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref previousRef, x - (Vector128<int>.Count - 1)));

    ////    // Vector LoadUnsafe ✓
    ////    var insertCost = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref previousRef, x - Vector128<int>.Count));

    ////    // Vector.Min( ✓
    ////    var localCost = Vector128.Min(
    ////        Vector128.Min(insertCost, deleteCost),
    ////        substitutionCost);
    ////    ////Vector128.Create(Scalar<int>.One);
    ////    // Vector + ✓
    ////    localCost += Vector128<int>.One;// Vector128.Create(1);

    ////    // Vector Store ✓
    ////    localCost.StoreUnsafe(
    ////        ref Unsafe.Add(ref currentRef, x - (Vector128<int>.Count - 1)));
    ////}

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceSSE2(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var costLen = xWord.Length + 1;
    ////    ////RuntimeHelpers.GetUninitializedObject()
    ////    // _mm_cmpeq_epi32
    ////    var costArray = new int[costLen * 2];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
    ////    ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
    ////    for (int i = 0; i < costLen; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }
    ////    ////Unsafe.CopyBlockUnaligned()

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
    ////    ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    var endX = 0;
    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        // this isn't really needed after (i > yLen) but its better than a branch?
    ////        previousRef = i;

    ////        //startX += i <= xWord.Length ? 1 : 0;
    ////        if (i <= xWord.Length)
    ////        {
    ////            //++startX;
    ////            startX = i;
    ////        }

    ////        if (i > yWord.Length)
    ////        {
    ////            ++endX;// = i - yWord.Length;
    ////        }

    ////        ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

    ////        var sseSize = (startX - endX) / Vector128<int>.Count;

    ////        var x = startX;
    ////        var yInt = (i - startX) + 1;
    ////        for (int j = 0; j < sseSize; j++)
    ////        {
    ////            DistanceSSERow2(
    ////                ref previousRef,
    ////                ref currentRef,
    ////                ref xWordRef,
    ////                ref yWordRef,
    ////                x,
    ////                yInt);

    ////            x -= Vector128<int>.Count;
    ////            y = ref Unsafe.Add(ref y, Vector128<int>.Count);
    ////            yInt += Vector128<int>.Count;
    ////        }

    ////        for (; x > endX; x--)
    ////        {
    ////            var replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (Unsafe.Add(ref xWordRef, x - 1) != y)
    ////            {
    ////                var deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                var insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y = ref Unsafe.Add(ref y, 1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////[MethodImpl(MethodImplOptions.AggressiveInlining)]
    ////private static void DistanceSSERow2(
    ////    ref int previousRef,
    ////    ref int currentRef,
    ////    ref char xString,
    ////    ref char yString,
    ////    int x,
    ////    int y)
    ////{

    ////    ////fixed (char* sourcePtr = value1)
    ////    ////fixed (char* targetPtr = value2)
    ////    var sourceVector = IntFromChar2(ref xString, x - Vector128<int>.Count);// Sse41.ConvertToVector128Int32((ushort*)sourcePtr + rowIndex - Vector128<int>.Count);
    ////    var targetVector = IntFromCharReverse2(ref yString, y - 1);// Sse41.ConvertToVector128Int32((ushort*)targetPtr + columnIndex - 1);
    ////                                                               //// targetVector = Vector128.Shuffle(targetVector, Vector128.LoadUnsafe(ref reverse));
    ////                                                               //// targetVector = Sse2.Shuffle(targetVector, 0x1b);

    ////    var substitutionCostAdjustment = Sse2.CompareEqual(sourceVector, targetVector);

    ////    var diag1Vector = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref currentRef, x - Vector128<int>.Count));

    ////    var substitutionCost = diag1Vector + substitutionCostAdjustment;

    ////    var deleteCost = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref previousRef, x - (Vector128<int>.Count - 1)));

    ////    var insertCost = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref previousRef, x - Vector128<int>.Count));

    ////    var localCost = Vector128.Min(
    ////        Vector128.Min(insertCost, deleteCost),
    ////        substitutionCost);

    ////    localCost += Vector128<int>.One;// Vector128.Create(1);

    ////    localCost.StoreUnsafe(
    ////        ref Unsafe.Add(ref currentRef, x - (Vector128<int>.Count - 1)));
    ////}

    ////private static Vector128<int> IntFromChar2(ref char value, int index)
    ////{
    ////    ////if (Sse41.IsSupported)
    ////    ////{
    ////    //    fixed (char* valuePtr = value)
    ////    //    {
    ////    ////return Sse41.ConvertToVector128Int32((ushort*)valuePtr + index);
    ////    ////    }
    ////    ////}
    ////    ///
    ////    //ref var ssssrer = ref (ref ushort)value;
    ////    ref var shortValue = ref Unsafe.As<char, ushort>(ref value);
    ////    shortValue = ref Unsafe.Add(ref shortValue, index);
    ////    var shortSpan = MemoryMarshal.CreateSpan(ref shortValue, 8);

    ////    //var smallSpan = MemoryMarshal.CreateSpan(ref Unsafe.Add(ref value, index), 8);
    ////    ////var sortSpan = MemoryMarshal.Cast<char, ushort>(smallSpan);
    ////    //////sortSpan.Non
    ////    ////ref var ssewewe = ref smallSpan;
    ////    ////ref var sssss = ref Unsafe.As<Span<char>, Span<ushort>>(ref smallSpan);
    ////    var shortVector = Vector128.Create<ushort>(shortSpan);
    ////    var intVetor = Vector128.WidenLower(shortVector);
    ////    return intVetor.AsInt32();

    ////    ////Span<ushort> tmp = stackalloc ushort[8];
    ////    ////tmp[3] = Unsafe.Add(ref value, index + 3);
    ////    ////tmp[2] = Unsafe.Add(ref value, index + 2);
    ////    ////tmp[1] = Unsafe.Add(ref value, index + 1);
    ////    ////tmp[0] = Unsafe.Add(ref value, index);

    ////    ////var shortVector = Vector128.Create<ushort>(tmp);
    ////    ////var intVetor = Vector128.WidenLower(shortVector);
    ////    ////return intVetor.AsInt32();
    ////    // Unsafe.As<Vector128<TFrom>, Vector128<TTo>>(ref vector);

    ////    //var charSpan = value.AsSpan(index, 4);
    ////    //var shortSpan = MemoryMarshal.Cast<char, ushort>(charSpan);
    ////    //ref var spanRef = ref MemoryMarshal.GetReference(shortSpan);
    ////    //var shortVector = Vector64.LoadUnsafe(ref spanRef);
    ////    //var v128 = shortVector.ToVector128Unsafe();
    ////    //var intVector = Vector128.WidenLower(v128);
    ////    //return intVector.As<uint, int>();
    ////}

    ////private static Vector128<int> IntFromCharReverse2(ref char value, int index)
    ////{
    ////    ref var shortValue = ref Unsafe.As<char, ushort>(ref value);
    ////    shortValue = ref Unsafe.Add(ref shortValue, index);
    ////    var shortSpan = MemoryMarshal.CreateSpan(ref shortValue, 8);
    ////    var shortVector = Vector128.Create<ushort>(shortSpan);
    ////    var intVetor = Vector128.WidenLower(shortVector);
    ////    intVetor = Sse2.Shuffle(intVetor, 0x1b);
    ////    return intVetor.AsInt32();

    ////    ////Span<int> tmp = stackalloc int[4];
    ////    ////tmp[0] = Unsafe.Add(ref value, index + 3);
    ////    ////tmp[1] = Unsafe.Add(ref value, index + 2);
    ////    ////tmp[2] = Unsafe.Add(ref value, index + 1);
    ////    ////tmp[3] = Unsafe.Add(ref value, index);

    ////    ////return Vector128.Create<int>(tmp);
    ////}

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceSSE(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var costLen = xWord.Length + 1;
    ////    var costArray = new int[costLen * 2];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
    ////    ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
    ////    for (int i = 0; i < costLen; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
    ////    var endX = 0;
    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        // this isn't really needed after (i > yLen) but its better than a branch?
    ////        previousRef = i;

    ////        //startX += i <= xWord.Length ? 1 : 0;
    ////        if (i <= xWord.Length)
    ////        {
    ////            //++startX;
    ////            startX = i;
    ////        }

    ////        if (i > yWord.Length)
    ////        {
    ////            ++endX;// = i - yWord.Length;
    ////        }

    ////        ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

    ////        var sseSize = (startX - endX) / Vector128<int>.Count;

    ////        var x = startX;
    ////        var yInt = (i - startX) + 1;
    ////        for (int j = 0; j < sseSize; j++)
    ////        {
    ////            DistanceSSERow(
    ////                ref currentRef,
    ////                ref previousRef,
    ////                xWord,
    ////                yWord,
    ////                x,
    ////                yInt);

    ////            x -= Vector128<int>.Count;
    ////            y = ref Unsafe.Add(ref y, Vector128<int>.Count);
    ////            yInt += Vector128<int>.Count;
    ////        }

    ////        for (; x > endX; x--)
    ////        {
    ////            var replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (Unsafe.Add(ref xWordRef, x - 1) != y)
    ////            {
    ////                var deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                var insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y = ref Unsafe.Add(ref y, 1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////[MethodImpl(MethodImplOptions.AggressiveInlining)]
    ////private static void DistanceSSERow(
    ////    ref int currentRef,
    ////    ref int previousRef,
    ////    string xString,
    ////    string yString,
    ////    int x,
    ////    int y)
    ////{

    ////    ////fixed (char* sourcePtr = value1)
    ////    ////fixed (char* targetPtr = value2)
    ////    var sourceVector = IntFromChar(xString, x - Vector128<int>.Count);// Sse41.ConvertToVector128Int32((ushort*)sourcePtr + rowIndex - Vector128<int>.Count);
    ////    var targetVector = IntFromCharReverse(yString, y - 1);// Sse41.ConvertToVector128Int32((ushort*)targetPtr + columnIndex - 1);
    ////                                                          //// targetVector = Vector128.Shuffle(targetVector, Vector128.LoadUnsafe(ref reverse));
    ////                                                          //// targetVector = Sse2.Shuffle(targetVector, 0x1b);

    ////    var substitutionCostAdjustment = Sse2.CompareEqual(sourceVector, targetVector);

    ////    var diag1Vector = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref currentRef, x - Vector128<int>.Count));

    ////    var substitutionCost = diag1Vector + substitutionCostAdjustment;

    ////    var deleteCost = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref previousRef, x - (Vector128<int>.Count - 1)));

    ////    var insertCost = Vector128.LoadUnsafe(
    ////        ref Unsafe.Add(ref previousRef, x - Vector128<int>.Count));

    ////    var localCost = Vector128.Min(
    ////        Vector128.Min(insertCost, deleteCost),
    ////        substitutionCost);

    ////    localCost += Vector128.Create(1);

    ////    localCost.StoreUnsafe(
    ////        ref Unsafe.Add(ref currentRef, x - (Vector128<int>.Count - 1)));
    ////}

    ////private static Vector128<int> IntFromChar(string value, int index)
    ////{
    ////    ////if (Sse41.IsSupported)
    ////    ////{
    ////    ////    fixed (char* valuePtr = value)
    ////    ////    {
    ////    ////        return Sse41.ConvertToVector128Int32((ushort*)valuePtr + index);
    ////    ////    }
    ////    ////}
    ////    Span<ushort> tmp = stackalloc ushort[8];
    ////    tmp[3] = value[index + 3];
    ////    tmp[2] = value[index + 2];
    ////    tmp[1] = value[index + 1];
    ////    tmp[0] = value[index];

    ////    var shortVector = Vector128.Create<ushort>(tmp);
    ////    var intVetor = Vector128.WidenLower(shortVector);
    ////    return intVetor.AsInt32();
    ////    // Unsafe.As<Vector128<TFrom>, Vector128<TTo>>(ref vector);

    ////    //var charSpan = value.AsSpan(index, 4);
    ////    //var shortSpan = MemoryMarshal.Cast<char, ushort>(charSpan);
    ////    //ref var spanRef = ref MemoryMarshal.GetReference(shortSpan);
    ////    //var shortVector = Vector64.LoadUnsafe(ref spanRef);
    ////    //var v128 = shortVector.ToVector128Unsafe();
    ////    //var intVector = Vector128.WidenLower(v128);
    ////    //return intVector.As<uint, int>();
    ////}

    ////private static Vector128<int> IntFromCharReverse(string value, int index)
    ////{
    ////    Span<int> tmp = stackalloc int[4];
    ////    tmp[0] = value[index + 3];
    ////    tmp[1] = value[index + 2];
    ////    tmp[2] = value[index + 1];
    ////    tmp[3] = value[index];

    ////    return Vector128.Create<int>(tmp);
    ////}

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int Distance(string xWord, string yWord)
    {
        if (xWord.Length == 0)
        {
            return yWord.Length;
        }

        var costLen = xWord.Length + 1;
        var costArray = new int[costLen * 2];
        ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
        ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
        for (int i = 0; i < costLen; i++)
        {
            Unsafe.Add(ref previousRef, i) = i;
            Unsafe.Add(ref currentRef, i) = i;
        }

        int startX = 0;
        var max = xWord.Length + yWord.Length;

        ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
        ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
        var endX = 0;
        for (int i = 1; i < max; ++i)
        {
            // this isn't really needed after (i > yLen) but its better than a branch?
            previousRef = i;

            //startX += i <= xWord.Length ? 1 : 0;
            if (i <= xWord.Length)
            {
                //++startX;
                startX = i;
            }

            if (i > yWord.Length)
            {
                ++endX;// = i - yWord.Length;
            }

            ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

            for (var x = startX; x > endX; x--)
            {
                var replacementCost = Unsafe.Add(ref currentRef, x - 1);

                if (Unsafe.Add(ref xWordRef, x - 1) != y)
                {
                    var deletionCost = Unsafe.Add(ref previousRef, x - 1);
                    if (deletionCost < replacementCost)
                    {
                        replacementCost = deletionCost;
                    }

                    var insertionCost = Unsafe.Add(ref previousRef, x);
                    if (insertionCost < replacementCost)
                    {
                        replacementCost = insertionCost;
                    }

                    ++replacementCost;
                }

                Unsafe.Add(ref currentRef, x) = replacementCost;
                y = ref Unsafe.Add(ref y, 1);
            }

            ref var tmp = ref previousRef;
            previousRef = ref currentRef;
            currentRef = ref tmp;
        }

        return Unsafe.Add(ref previousRef, xWord.Length);
    }

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceQuickCosting(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var costLen = xWord.Length + 1;
    ////    var costArray = new int[costLen * 2];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
    ////    ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
    ////    for (int i = 0; i < costLen; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
    ////    var endX = 0;
    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        // this isn't really needed after (i > yLen) but its better than a branch?
    ////        previousRef = i;

    ////        //startX += i <= xWord.Length ? 1 : 0;
    ////        if (i <= xWord.Length)
    ////        {
    ////            //++startX;
    ////            startX = i;
    ////        }

    ////        if (i > yWord.Length)
    ////        {
    ////            ++endX;// = i - yWord.Length;
    ////        }

    ////        ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

    ////        var lastInsertionCost = Unsafe.Add(ref currentRef, startX - 1);
    ////        var lastSubstitutionCost = Unsafe.Add(ref previousRef, startX);

    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            var localCost = Math.Min(
    ////                Unsafe.Add(ref previousRef, x),
    ////                Unsafe.Add(ref previousRef, x - 1));

    ////            if (localCost < Unsafe.Add(ref currentRef, x - 1))
    ////            {
    ////                Unsafe.Add(ref currentRef, x) = localCost + 1;
    ////            }
    ////            else
    ////            {
    ////                Unsafe.Add(ref currentRef, x) = Unsafe.Add(ref currentRef, x - 1) + (Unsafe.Add(ref xWordRef, x - 1) != y ? 1 : 0);
    ////            }

    ////            y = ref Unsafe.Add(ref y, 1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceRetroMental5(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var costLen = xWord.Length + 1;
    ////    var costArray = new int[costLen * 2];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
    ////    ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
    ////    for (int i = 0; i < costLen; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
    ////    var endX = 0;
    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        // this isn't really needed after (i > yLen) but its better than a branch?
    ////        previousRef = i;

    ////        //startX += i <= xWord.Length ? 1 : 0;
    ////        //startX += i <= xWord.Length ? 1 : 0;
    ////        if (i <= xWord.Length)
    ////        {
    ////            ++startX;
    ////            //startX = i;
    ////        }

    ////        //endX += i > yWord.Length ? 1 : 0;

    ////        if (i > yWord.Length)
    ////        {
    ////            ++endX;// = i - yWord.Length;
    ////        }

    ////        ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            var replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (Unsafe.Add(ref xWordRef, x - 1) != y)
    ////            {
    ////                var deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                var insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y = ref Unsafe.Add(ref y, 1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceRetroMental4(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var costLen = xWord.Length + 1;
    ////    var costArray = new int[costLen * 2];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
    ////    ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
    ////    for (int i = 0; i < costLen; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
    ////    ref var yStart = ref yWordRef;
    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        // this isn't really needed after (i > yLen) but its better than a branch?

    ////        if (i <= xWord.Length)
    ////        {
    ////            startX = i;
    ////        }
    ////        else
    ////        {
    ////            // i - startX);
    ////            yStart = ref Unsafe.Add(ref yStart, 1);
    ////        }

    ////        var endX = 0;
    ////        if (i > yWord.Length)
    ////        {
    ////            endX = i - yWord.Length;
    ////        }
    ////        else
    ////        {
    ////            previousRef = i;
    ////        }

    ////        ref var y = ref yStart;
    ////        ////startX += i <= xWord.Length ? 1 : 0;
    ////        //////if (i <= xWord.Length)
    ////        //////{
    ////        //////    //++startX;
    ////        //////    startX = i;
    ////        //////}

    ////        ////endX += i > yWord.Length ? 1 : 0;
    ////        //////if (i > yWord.Length)
    ////        //////{
    ////        //////    ++endX;// = i - yWord.Length;
    ////        //////}
    ////        ref var xWordRef2 = ref Unsafe.Add(ref xWordRef, startX - 1);
    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            var replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (xWordRef2 != y)
    ////            {
    ////                var deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                var insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y = ref Unsafe.Add(ref y, 1);
    ////            xWordRef2 = ref Unsafe.Add(ref xWordRef2, -1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceRetroMental3(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var costLen = xWord.Length + 1;
    ////    var costArray = new int[costLen * 2];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
    ////    ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
    ////    for (int i = 0; i < costLen; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    ref char xWordRef = ref MemoryMarshal.GetReference(xWord.AsSpan());
    ////    var endX = 0;
    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        // this isn't really needed after (i > yLen) but its better than a branch?


    ////        if (i <= xWord.Length)
    ////        {
    ////            ++startX;
    ////        }


    ////        if (i > yWord.Length)
    ////        {
    ////            ++endX;
    ////        }
    ////        else
    ////        {
    ////            previousRef = i;
    ////        }
    ////        ////startX += i <= xWord.Length ? 1 : 0;
    ////        //////if (i <= xWord.Length)
    ////        //////{
    ////        //////    //++startX;
    ////        //////    startX = i;
    ////        //////}

    ////        ////endX += i > yWord.Length ? 1 : 0;
    ////        //////if (i > yWord.Length)
    ////        //////{
    ////        //////    ++endX;// = i - yWord.Length;
    ////        //////}

    ////        ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            var replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (Unsafe.Add(ref xWordRef, x - 1) != y)
    ////            {
    ////                var deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                var insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y = ref Unsafe.Add(ref y, 1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceRetroMental(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var costLen = xWord.Length + 1;
    ////    var costArray = new int[costLen * 2];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(costArray);
    ////    ref int currentRef = ref Unsafe.Add(ref previousRef, costLen);
    ////    for (int i = 0; i < costLen; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    ref readonly char yWordReadRef = ref yWord.GetPinnableReference();// ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    ref char yWordRef = ref Unsafe.AsRef(in yWordReadRef);

    ////    ref readonly char xWordReadRef = ref xWord.GetPinnableReference();// ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    ref char xWordRef = ref Unsafe.AsRef(in xWordReadRef);

    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        // this isn't really needed after (i > yLen) but its better than a branch?
    ////        previousRef = i;

    ////        if (i <= xWord.Length)
    ////        {
    ////            startX = i;
    ////        }

    ////        var endX = 0;
    ////        if (i > yWord.Length)
    ////        {
    ////            endX = i - yWord.Length;
    ////        }

    ////        ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            var replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (Unsafe.Add(ref xWordRef, x - 1) != y)
    ////            {
    ////                var deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                var insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y = ref Unsafe.Add(ref y, 1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceRetro(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    if (yWord.Length == 0)
    ////    {
    ////        return xWord.Length;
    ////    }

    ////    var previous = new int[xWord.Length + 1];
    ////    var current = new int[xWord.Length + 1];
    ////    ref int previousRef = ref MemoryMarshal.GetArrayDataReference(previous);
    ////    ref int currentRef = ref MemoryMarshal.GetArrayDataReference(current);

    ////    for (int i = 0; i < previous.Length; i++)
    ////    {
    ////        Unsafe.Add(ref previousRef, i) = i;
    ////        Unsafe.Add(ref currentRef, i) = i;
    ////    }

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;


    ////    ref readonly char yWordReadRef = ref yWord.GetPinnableReference();// ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    ref char yWordRef = ref Unsafe.AsRef(in yWordReadRef);

    ////    ref readonly char xWordReadRef = ref xWord.GetPinnableReference();// ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////    ref char xWordRef = ref Unsafe.AsRef(in xWordReadRef);

    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        // this isn't really needed after (i > yLen) but its better than a branch?
    ////        previousRef = i;

    ////        if (i <= xWord.Length)
    ////        {
    ////            startX = i;
    ////        }

    ////        var endX = 0;
    ////        if (i > yWord.Length)
    ////        {
    ////            endX = i - yWord.Length;
    ////        }

    ////        ref var y = ref Unsafe.Add(ref yWordRef, i - startX);

    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            var replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (Unsafe.Add(ref xWordRef, x - 1) != y)
    ////            {
    ////                var deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                var insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y = ref Unsafe.Add(ref y, 1);
    ////        }

    ////        ref var tmp = ref previousRef;
    ////        previousRef = ref currentRef;
    ////        currentRef = ref tmp;
    ////    }

    ////    return Unsafe.Add(ref previousRef, xWord.Length);
    ////}

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int Distance(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    if (yWord.Length == 0)
    ////    {
    ////        return xWord.Length;
    ////    }

    ////    var previous = new int[xWord.Length + 1];
    ////    var current = new int[xWord.Length + 1];

    ////    for (int i = 0; i < previous.Length; i++)
    ////    {
    ////        previous[i] = i;
    ////        current[i] = i;
    ////    }

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;


    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        ref int previousRef = ref MemoryMarshal.GetArrayDataReference(previous);
    ////        ref int currentRef = ref MemoryMarshal.GetArrayDataReference(current);

    ////        // this isn't really needed after (i > yLen) but its better than a branch?
    ////        previousRef = i;

    ////        if (i <= xWord.Length)
    ////        {
    ////            startX = i;
    ////        }

    ////        var endX = 0;
    ////        if (i > yWord.Length)
    ////        {
    ////            endX = i - yWord.Length;
    ////        }

    ////        var y = i - startX;

    ////        ref readonly char yWordReadRef = ref yWord.GetPinnableReference();// ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////        ref char yWordRef = ref Unsafe.AsRef(in yWordReadRef);

    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            // todo: host this out and keep last time
    ////            int replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (xWord[x - 1] != Unsafe.Add(ref yWordRef, y))
    ////            {
    ////                int deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                int insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y++;
    ////        }

    ////        var tmp = previous;
    ////        previous = current;
    ////        current = tmp;
    ////    }

    ////    return previous[previous.Length - 1];
    ////}

    ////[MethodImpl(MethodImplOptions.NoInlining)]
    ////public static int DistanceWithYRef(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    if (yWord.Length == 0)
    ////    {
    ////        return xWord.Length;
    ////    }

    ////    var previous = new int[xWord.Length + 1];
    ////    var current = new int[xWord.Length + 1];

    ////    for (int i = 0; i < previous.Length; i++)
    ////    {
    ////        previous[i] = i;
    ////        current[i] = i;
    ////    }

    ////    int startX = 0;
    ////    var max = xWord.Length + yWord.Length;


    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        ref int previousRef = ref MemoryMarshal.GetArrayDataReference(previous);
    ////        ref int currentRef = ref MemoryMarshal.GetArrayDataReference(current);

    ////        // this isn't really needed after (i > yLen) but its better than a branch?
    ////        previousRef = i;

    ////        if (i <= xWord.Length)
    ////        {
    ////            startX = i;
    ////        }

    ////        var endX = 0;
    ////        if (i > yWord.Length)
    ////        {
    ////            endX = i - yWord.Length;
    ////        }

    ////        var y = i - startX;

    ////        ref char yWordRef = ref MemoryMarshal.GetReference(yWord.AsSpan());
    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            // todo: host this out and keep last time
    ////            int replacementCost = Unsafe.Add(ref currentRef, x - 1);

    ////            if (xWord[x - 1] != Unsafe.Add(ref yWordRef, y))
    ////            {
    ////                int deletionCost = Unsafe.Add(ref previousRef, x - 1);
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                int insertionCost = Unsafe.Add(ref previousRef, x);
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            Unsafe.Add(ref currentRef, x) = replacementCost;
    ////            y++;
    ////        }

    ////        var tmp = previous;
    ////        previous = current;
    ////        current = tmp;
    ////    }

    ////    return previous[previous.Length - 1];
    ////}

    ////public static int DistanceFastenshteinCost(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    if (yWord.Length == 0)
    ////    {
    ////        return xWord.Length;
    ////    }

    ////    var previous = new int[xWord.Length + 1];
    ////    var current = new int[xWord.Length + 1];

    ////    for (int i = 0; i < previous.Length; i++)
    ////    {
    ////        previous[i] = i;
    ////        current[i] = i;
    ////    }

    ////    var startX = 1;
    ////    var endX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        previous[0] = i;

    ////        var y = (i - startX);

    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            int replacementCost = current[x - 1];

    ////            if (xWord[x - 1] != yWord[y])
    ////            {
    ////                int deletionCost = previous[x - 1];
    ////                if (deletionCost < replacementCost)
    ////                {
    ////                    replacementCost = deletionCost;
    ////                }

    ////                int insertionCost = previous[x];
    ////                if (insertionCost < replacementCost)
    ////                {
    ////                    replacementCost = insertionCost;
    ////                }

    ////                ++replacementCost;
    ////            }

    ////            current[x] = replacementCost;

    ////            y++;
    ////        }

    ////        if (i < xWord.Length)
    ////        {
    ////            startX++;
    ////        }

    ////        if (i >= yWord.Length)
    ////        {
    ////            ++endX;
    ////        }

    ////        var tmp = previous;
    ////        previous = current;
    ////        current = tmp;
    ////    }

    ////    return previous[previous.Length - 1];
    ////}

    ////public static int DistanceBranchlessDoubleXNoCounters(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    if (yWord.Length == 0)
    ////    {
    ////        return xWord.Length;
    ////    }

    ////    var previous = new int[xWord.Length + 1];
    ////    var current = new int[xWord.Length + 1];

    ////    for (int i = 0; i < previous.Length; i++)
    ////    {
    ////        previous[i] = i;
    ////        current[i] = i;
    ////    }

    ////    var startX = 1;
    ////    var endX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        previous[0] = i;

    ////        var y = (i - startX);

    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            var cost = (xWord[x - 1] == yWord[y]) ? 0 : 1;

    ////            var value = Math.Min(
    ////                current[x - 1] + cost,
    ////                Math.Min(previous[x] + 1, previous[x - 1] + 1));

    ////            current[x] = value;

    ////            y++;
    ////        }

    ////        var xTmp = (i - xWord.Length) >> 31 & 1;
    ////        startX += xTmp;

    ////        var yTmp = (yWord.Length - i - 1) >> 31 & 1;
    ////        endX += yTmp;

    ////        var tmp = previous;
    ////        previous = current;
    ////        current = tmp;
    ////    }

    ////    return previous[previous.Length - 1];
    ////}

    ////public static int DistanceBranchlessDoubleX(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    if (yWord.Length == 0)
    ////    {
    ////        return xWord.Length;
    ////    }

    ////    var previous = new int[xWord.Length + 1];
    ////    var current = new int[xWord.Length + 1];

    ////    for (int i = 0; i < previous.Length; i++)
    ////    {
    ////        previous[i] = i;
    ////        current[i] = i;
    ////    }

    ////    var startX = 1;
    ////    var endX = 0;
    ////    var max = xWord.Length + yWord.Length;
    ////    var xWordCounter = -xWord.Length;
    ////    var yWordCounter = yWord.Length - 1;

    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        previous[0] = i;

    ////        var y = (i - startX);

    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            var cost = (xWord[x - 1] == yWord[y]) ? 0 : 1;

    ////            var value = Math.Min(
    ////                current[x - 1] + cost,
    ////                Math.Min(previous[x] + 1, previous[x - 1] + 1));

    ////            current[x] = value;

    ////            y++;
    ////        }

    ////        ++xWordCounter;
    ////        var xTmp = xWordCounter >> 31 & 1;
    ////        startX += xTmp;

    ////        --yWordCounter;
    ////        var yTmp = yWordCounter >> 31 & 1;
    ////        endX += yTmp;

    ////        var tmp = previous;
    ////        previous = current;
    ////        current = tmp;
    ////    }

    ////    return previous[previous.Length - 1];
    ////}

    ////public static int DistanceBranchlessX(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    if (yWord.Length == 0)
    ////    {
    ////        return xWord.Length;
    ////    }

    ////    var previous = new int[xWord.Length + 1];
    ////    var current = new int[xWord.Length + 1];

    ////    for (int i = 0; i < previous.Length; i++)
    ////    {
    ////        previous[i] = i;
    ////        current[i] = i;
    ////    }

    ////    var startX = 1;
    ////    var endX = 0;
    ////    var max = xWord.Length + yWord.Length;
    ////    var yWordCounter = yWord.Length - 1;

    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        previous[0] = i;

    ////        var y = (i - startX);

    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            var cost = (xWord[x - 1] == yWord[y]) ? 0 : 1;

    ////            var value = Math.Min(
    ////                current[x - 1] + cost,
    ////                Math.Min(previous[x] + 1, previous[x - 1] + 1));

    ////            current[x] = value;

    ////            y++;
    ////        }

    ////        if (i < xWord.Length)
    ////        {
    ////            startX++;
    ////        }

    ////        --yWordCounter;
    ////        var yTmp = yWordCounter >> 31 & 1;
    ////        endX += yTmp;

    ////        var tmp = previous;
    ////        previous = current;
    ////        current = tmp;

    ////    }

    ////    return previous[previous.Length - 1];
    ////}

    ////public static int DistanceWithX_NonIncrement(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    if (yWord.Length == 0)
    ////    {
    ////        return xWord.Length;
    ////    }

    ////    var previous = new int[xWord.Length + 1];
    ////    var current = new int[xWord.Length + 1];

    ////    for (int i = 0; i < previous.Length; i++)
    ////    {
    ////        previous[i] = i;
    ////        current[i] = i;
    ////    }

    ////    var startX = 1;
    ////    var endX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        previous[0] = i;

    ////        var y = (i - startX);

    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            var cost = (xWord[x - 1] == yWord[y]) ? 0 : 1;

    ////            var value = Math.Min(
    ////                current[x - 1] + cost,
    ////                Math.Min(previous[x] + 1, previous[x - 1] + 1));

    ////            current[x] = value;

    ////            y++;
    ////        }

    ////        if (i < xWord.Length)
    ////        {
    ////            startX++;
    ////        }

    ////        var yy = i - (yWord.Length - 1);
    ////        if (yy > 0)
    ////        {
    ////            endX = yy;
    ////        }

    ////        var tmp = previous;
    ////        previous = current;
    ////        current = tmp;
    ////    }

    ////    return previous[previous.Length - 1];
    ////}

    ////public static int DistanceWith_X_Min_Max(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    if (yWord.Length == 0)
    ////    {
    ////        return xWord.Length;
    ////    }

    ////    var previous = new int[xWord.Length + 1];
    ////    var current = new int[xWord.Length + 1];

    ////    for (int i = 0; i < previous.Length; i++)
    ////    {
    ////        previous[i] = i;
    ////        current[i] = i;
    ////    }

    ////    var startX = 1;
    ////    var endX = 0;
    ////    var max = xWord.Length + yWord.Length;

    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        previous[0] = i;

    ////        var y = (i - startX);

    ////        for (var x = startX; x > endX; x--)
    ////        {
    ////            var cost = (xWord[x - 1] == yWord[y]) ? 0 : 1;

    ////            var value = Math.Min(
    ////                current[x - 1] + cost,
    ////                Math.Min(previous[x] + 1, previous[x - 1] + 1));

    ////            current[x] = value;

    ////            y++;
    ////        }

    ////        if (i < xWord.Length)
    ////        {
    ////            startX++;
    ////        }

    ////        if (i >= yWord.Length)
    ////        {
    ////            ++endX;
    ////        }

    ////        var tmp = previous;
    ////        previous = current;
    ////        current = tmp;
    ////    }

    ////    return previous[previous.Length - 1];
    ////}

    ////public static int Distance2(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var previous = new int[xWord.Length + 1];
    ////    var current = new int[xWord.Length + 1];

    ////    for (int i = 0; i < previous.Length; i++)
    ////    {
    ////        previous[i] = i;
    ////        current[i] = i;
    ////    }

    ////    var startX = 1;
    ////    var max = xWord.Length + yWord.Length;

    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        previous[0] = i;

    ////        var y = (i - startX);
    ////        var x = startX;

    ////        ///var otherY = (i - startX);
    ////        //var xEnd =   
    ////        //    if (i > yWord.Length)
    ////        //{
    ////        //    xMin = i - yWord.Length;
    ////        //}
    ////        //else
    ////        //{
    ////        //    xMin = 1;
    ////        //}

    ////        // could be for loop with x min/max
    ////        while (x > 0 && y < yWord.Length)
    ////        {
    ////            var cost = (xWord[x - 1] == yWord[y]) ? 0 : 1;

    ////            var value = Math.Min(
    ////                current[x - 1] + cost,
    ////                Math.Min(previous[x] + 1, previous[x - 1] + 1));

    ////            current[x] = value;

    ////            y++;
    ////            x--;
    ////        }

    ////        if (startX < xWord.Length)
    ////        {
    ////            startX++;
    ////        }


    ////        var tmp = previous;
    ////        previous = current;
    ////        current = tmp;
    ////    }

    ////    return previous[previous.Length - 1];
    ////}

    ////public static int DistanceOG(string xWord, string yWord)
    ////{
    ////    if (xWord.Length == 0)
    ////    {
    ////        return yWord.Length;
    ////    }

    ////    var previous = new int[xWord.Length + 1];
    ////    var current = new int[xWord.Length + 1];

    ////    for (int i = 0; i < previous.Length; i++)
    ////    {
    ////        previous[i] = i;
    ////        current[i] = i;
    ////    }

    ////    var startY = 0;
    ////    var startX = 1;
    ////    var max = xWord.Length + yWord.Length;

    ////    for (int i = 1; i < max; ++i)
    ////    {
    ////        previous[0] = i;

    ////        var y = startY;
    ////        var x = startX;

    ////        ///var otherY = (i - startX);
    ////        //var xEnd =   
    ////        //    if (i > yWord.Length)
    ////        //{
    ////        //    xMin = i - yWord.Length;
    ////        //}
    ////        //else
    ////        //{
    ////        //    xMin = 1;
    ////        //}

    ////        // could be for loop with x min/max
    ////        while (x > 0 && y < yWord.Length)
    ////        {
    ////            var cost = (xWord[x - 1] == yWord[y]) ? 0 : 1;

    ////            var value = Math.Min(
    ////                current[x - 1] + cost,
    ////                Math.Min(previous[x] + 1, previous[x - 1] + 1));

    ////            current[x] = value;

    ////            y++;
    ////            x--;
    ////        }

    ////        if (startX < xWord.Length)
    ////        {
    ////            startX++;
    ////        }
    ////        else
    ////        {
    ////            startY++;
    ////            // could not store this and just start bumming xMin
    ////        }

    ////        var tmp = previous;
    ////        previous = current;
    ////        current = tmp;
    ////    }

    ////    return previous[previous.Length - 1];
    ////}
}

#endif