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
        /// Compares the two values to find the minimum Levenshtein distance, stopping early if the distance exceeds maxDistance.
        /// Thread safe.
        /// </summary>
        /// <param name="value1">First string to compare.</param>
        /// <param name="value2">Second string to compare.</param>
        /// <param name="maxDistance">Maximum distance threshold.</param>
        /// <returns>Difference if less than or equal to maxDistance; otherwise -1.</returns>
        public static int Distance(string value1, string value2, int maxDistance)
        {
            if (maxDistance < 0)
            {
                return -1;
            }

            if (value1 == value2)
            {
                return 0;
            }

            int len1 = value1.Length;
            int len2 = value2.Length;

            if (len1 == 0)
            {
                return len2 <= maxDistance ? len2 : -1;
            }

            if (len2 == 0)
            {
                return len1 <= maxDistance ? len1 : -1;
            }

            int lenDiff = len2 - len1;
            if (System.Math.Abs(lenDiff) > maxDistance)
            {
                return -1;
            }

            int[] costs = new int[len2];

            for (int j = 0; j < costs.Length; j++)
            {
                int val = j + 1;
                costs[j] = val <= maxDistance ? val : maxDistance + 1;
            }

            for (int i = 0; i < len1; i++)
            {
                char value1Char = value1[i];
                int r = i + 1;

                int minDiag = System.Math.Max(-maxDistance, lenDiff - maxDistance);
                int maxDiag = System.Math.Min(maxDistance, lenDiff + maxDistance);

                int startC = System.Math.Max(1, r + minDiag);
                int endC = System.Math.Min(len2, r + maxDiag);

                if (startC > endC)
                {
                    return -1;
                }

                int startJ = startC - 1;
                int endJ = endC - 1;

                int diagAboveLeft = (startJ == 0) ? i : costs[startJ - 1];
                int leftCost = (startJ == 0) ? r : maxDistance + 1;

                int rowMin = int.MaxValue;

                for (int j = startJ; j <= endJ; j++)
                {
                    int aboveCost = costs[j];
                    int currentCost;

                    if (value1Char == value2[j])
                    {
                        currentCost = diagAboveLeft;
                    }
                    else
                    {
                        currentCost = diagAboveLeft < aboveCost ? diagAboveLeft : aboveCost;
                        if (leftCost < currentCost)
                        {
                            currentCost = leftCost;
                        }

                        ++currentCost;
                    }

                    diagAboveLeft = aboveCost;
                    leftCost = currentCost;
                    costs[j] = currentCost;

                    if (currentCost < rowMin)
                    {
                        rowMin = currentCost;
                    }
                }

                if (rowMin > maxDistance)
                {
                    return -1;
                }
            }

            int result = costs[len2 - 1];
            return result <= maxDistance ? result : -1;
        }
    }
}
