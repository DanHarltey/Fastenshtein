using System;
using System.Numerics;

namespace Fastenshtein.Benchmarking.FastenshteinOld;

/// <summary>
/// Measures the difference between two strings.
/// Uses the Levenshtein string difference algorithm.
/// </summary>
internal sealed class Fastenshtein_1_1_1
{
    /*
     * WARNING this class is performance critical (Speed).
     */

    private readonly string _storedValue;
    private readonly int[] _costs;

    /// <summary>
    /// Creates a new instance with a value to test other values against
    /// </summary>
    /// <param Name="value">Value to compare other values to.</param>
    public Fastenshtein_1_1_1(string value)
    {
        _storedValue = value;
        // Create matrix row
        _costs = new int[value.Length];
    }

    /// <summary>
    /// gets the length of the stored value that is tested against
    /// </summary>
    public int StoredLength => _storedValue.Length;

    /// <summary>
    /// Compares a value to the stored value. 
    /// Not thread safe.
    /// </summary>
    /// <returns>Difference. 0 complete match.</returns>
    public int DistanceFrom(string value)
    {
        // copying to local variables allows JIT to remove bounds checks, as it understands they can not change
        var costs = _costs;
        var storedValue = _storedValue;

        if (costs.Length == 0
            // this will never be ture, however it allows the JIT to remove a bounds check
            || costs.Length != storedValue.Length)
        {
            return value.Length;
        }

        // Add indexing for insertion to first row
        for (int i = 0; i < costs.Length;)
        {
            costs[i] = ++i;
        }

        for (int i = 0; i < value.Length; i++)
        {
            // cost of the first index
            int cost = i;
            int previousCost = i;

            // cache value for inner loop to avoid index lookup and bonds checking, profiled this is quicker
            char value1Char = value[i];

            for (int j = 0; j < storedValue.Length; j++)
            {
                int currentCost = cost;

                // assigning this here reduces the array reads we do, improvement of the old version
                cost = costs[j];

                if (value1Char != storedValue[j])
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

    /// <summary>
    /// Compares the two values to find the minimum Levenshtein distance. 
    /// Thread safe.
    /// </summary>
    /// <param name="value1">the first value to compare</param>
    /// <param name="value2">the second value to compare</param>
    /// <returns>The distance between <paramref name="value1"/> and <paramref name="value2"/>. Never negative. Zero is exact match. The higher value the greater the difference.</returns>
    public static int Distance(string value1, string value2)
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

    /// <summary>
    /// Compares the two values to find the minimum Levenshtein distance. 
    /// Thread safe.
    /// </summary>
    /// <returns>Difference. 0 complete match.</returns>
    public static int Distance<T, TOther>(ReadOnlySpan<T> value1, ReadOnlySpan<TOther> value2)
        where T : IEqualityOperators<T, TOther, bool>
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
            T value1Item = value1[i];

            for (int j = 0; j < value2.Length; j++)
            {
                int currentCost = cost;

                // assigning this here reduces the array reads we do, improvement of the old version
                cost = costs[j];

                if (value1Item != value2[j])
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

        return costs[^1];
    }
}