namespace Fastenshtein
{
    /// <summary>
    /// Measures the difference between two strings.
    /// Uses the Levenshtein string difference algorithm.
    /// </summary>
    public partial class Levenshtein
    {
        /*
         * WARRING this class is performance critical (Speed).
         */

        private readonly string _storedValue;
        private readonly int[] _costs;

        /// <summary>
        /// Creates a new instance with a value to test other values against
        /// </summary>
        /// <param Name="value">Value to compare other values to.</param>
        public Levenshtein(string value)
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
        /// Compares a value to the stored value, stopping early if the distance exceeds maxDistance.
        /// Not thread safe.
        /// </summary>
        /// <param name="value">Value to compare.</param>
        /// <param name="maxDistance">Maximum distance threshold.</param>
        /// <returns>Difference if less than or equal to maxDistance; otherwise -1.</returns>
        public int DistanceFrom(string value, int maxDistance)
        {
            if (maxDistance < 0)
            {
                return -1;
            }

            var costs = _costs;
            var storedValue = _storedValue;

            if (costs.Length == 0 || costs.Length != storedValue.Length)
            {
                return value.Length <= maxDistance ? value.Length : -1;
            }

            if (value.Length == 0)
            {
                return storedValue.Length <= maxDistance ? storedValue.Length : -1;
            }

            int lenDiff = storedValue.Length - value.Length;
            if (System.Math.Abs(lenDiff) > maxDistance)
            {
                return -1;
            }

            for (int j = 0; j < costs.Length; j++)
            {
                int val = j + 1;
                costs[j] = val <= maxDistance ? val : maxDistance + 1;
            }

            for (int i = 0; i < value.Length; i++)
            {
                char value1Char = value[i];
                int r = i + 1;

                int minDiag = System.Math.Max(-maxDistance, lenDiff - maxDistance);
                int maxDiag = System.Math.Min(maxDistance, lenDiff + maxDistance);

                int startC = System.Math.Max(1, r + minDiag);
                int endC = System.Math.Min(storedValue.Length, r + maxDiag);

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

                    if (value1Char == storedValue[j])
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

            int result = costs[costs.Length - 1];
            return result <= maxDistance ? result : -1;
        }
    }
}
