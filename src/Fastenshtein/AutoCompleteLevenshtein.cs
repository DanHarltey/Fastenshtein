namespace Fastenshtein
{
    /// <summary>
    /// Measures the difference between two strings.
    /// Uses the Levenshtein string difference algorithm.
    /// </summary>
    public static class AutoCompleteLevenshtein
    {
        /// <summary>
        /// For autocomplete-style matching. It calculates the distance between an incomplete value and the start of a candidate value.
        /// Thread safe.
        /// </summary>
        /// <param name="prefix">The incomplete value you want to match</param>
        /// <param name="candidate">The full candidate value to compare against</param>
        /// <returns>The distance between <paramref name="prefix"/> and <paramref name="candidate"/>. Never negative. Zero is exact match. The higher value the greater the difference.</returns>
        public static int Distance(string prefix, string candidate)
        {
            if (prefix.Length == 0)
            {
                return 0;
            }

            int[] costs = new int[prefix.Length];

            // Add indexing for insertion to first row
            for (int i = 0; i < costs.Length;)
            {
                costs[i] = ++i;
            }

            int minSize = prefix.Length < candidate.Length ? prefix.Length : candidate.Length;

            for (int i = 0; i < minSize; i++)
            {
                // cost of the first index
                int cost = i;
                int previousCost = i;

                // cache value for inner loop to avoid index lookup and bonds checking, profiled this is quicker
                char value2Char = candidate[i];

                for (int j = 0; j < prefix.Length; j++)
                {
                    int currentCost = cost;

                    // assigning this here reduces the array reads we do, improvement of the old version
                    cost = costs[j];

                    if (value2Char != prefix[j])
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
