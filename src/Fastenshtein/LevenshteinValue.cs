#if NET7_0_OR_GREATER

using System;
using System.Numerics;

namespace Fastenshtein;

/// <summary>
/// Measures the difference between two strings.
/// Uses the Levenshtein string difference algorithm.
/// </summary>
public readonly ref struct LevenshteinValue<T>
{
    /*
     * WARNING this class is performance critical (Speed).
     */

    private readonly ReadOnlySpan<T> _storedValue;
    private readonly Span<int> _costs;

    /// <summary>
    /// Creates a new instance with a value to test other values against
    /// </summary>
    /// <param Name="value">Value to compare other values to.</param>
    public LevenshteinValue(ReadOnlySpan<T> value)
    {
        _storedValue = value;
        // Create matrix row
        _costs = new int[value.Length];
    }

    /// <summary>
    /// Creates a new instance with a value to test other values against
    /// </summary>
    /// <param name="value">Value to compare other values to.</param>
    /// <param name="buffer">memory for the Levenstein operation to use. Must not be smaller than  <paramref name="value"/> in length.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="buffer"/> is smalller than <paramref name="value"/> in length</exception>
    public LevenshteinValue(ReadOnlySpan<T> value, Span<int> buffer)
    {
        if(value.Length > buffer.Length)
        {
            throw new ArgumentException(
                "Must not be below value in length",
                nameof(buffer));
        }

        _storedValue = value;
        _costs = buffer[.._storedValue.Length];
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
    public int DistanceFrom<TOther>(ReadOnlySpan<TOther> value)
        where TOther : IEqualityOperators<TOther, T, bool>
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
            TOther value1Char = value[i];

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
}
#endif