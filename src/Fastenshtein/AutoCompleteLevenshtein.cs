namespace Fastenshtein
{
#if !PCL
    using Microsoft.SqlServer.Server;
#endif

    /// <summary>
    /// Measures the difference between two strings.
    /// Uses the Levenshtein string difference algorithm.
    /// </summary>
    public static class AutoCompleteLevenshtein
    {
        /// <summary>
        /// Compares the two strings and returns a measure of their summarily with 0 being an exact match.
        /// </summary>
        /// <param name="value1">the incomplete value entered by the user</param>
        /// <param name="value2">the value to compare value1 against</param>
        /// <returns>0 exact match less a positive value, lower the value the best match</returns>
#if !PCL
        [SqlFunction(
            Name = "AutoCompleteLevenshteinDistance",
            DataAccess = DataAccessKind.None,
            SystemDataAccess = SystemDataAccessKind.None,
            IsDeterministic = true,
            IsPrecise = true)]
#endif
        public static int Distance(string value1, string value2)
        {
            if (value1.Length == 0)
            {
                return 0;
            }

            int[] costs = new int[value1.Length];

            // Add indexing for insertion to first row
            for (int i = 0; i < costs.Length;)
            {
                costs[i] = ++i;
            }

            int minSize = value1.Length < value2.Length ? value1.Length : value2.Length;

            for (int i = 0; i < minSize; i++)
            {
                // cost of the first index
                int cost = i;
                int addationCost = i;

                // cache value for inner loop to avoid index lookup and bonds checking, profiled this is quicker
                char value2Char = value2[i];

                for (int j = 0; j < value1.Length; j++)
                {
                    int insertionCost = cost;

                    cost = addationCost;

                    // assigning this here reduces the array reads we do, improvment of the old version
                    addationCost = costs[j];

                    if (value2Char != value1[j])
                    {
                        if (insertionCost < cost)
                        {
                            cost = insertionCost;
                        }

                        if (addationCost < cost)
                        {
                            cost = addationCost;
                        }

                        ++cost;
                    }

                    costs[j] = cost;
                }
            }

            return costs[costs.Length - 1];
        }
    }
}
