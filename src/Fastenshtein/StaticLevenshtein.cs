using System;

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
            return Distance(value1.AsSpan(), value2.AsSpan());
        }

        /// <summary>
        /// Compares the two values to find the minimum Levenshtein distance.
        /// Thread safe.
        /// </summary>
        /// <returns>Difference. 0 complete match.</returns>
        public static unsafe int Distance(ReadOnlySpan<char> value1, ReadOnlySpan<char> value2)
        {
            if (value2.Length == 0)
            {
                return value1.Length;
            }

            int costsLength = value2.Length;
            int* costs = stackalloc int[costsLength];

            // Add indexing for insertion to first row
            for (int i = 0; i < costsLength;)
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

            return costs[costsLength - 1];
        }
    }
}
