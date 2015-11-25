namespace Fastenshtein
{
    using Microsoft.SqlServer.Server;

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
        [SqlFunction(
            Name = "AutoCompleteLevenshteinDistance",
            DataAccess = DataAccessKind.None,
            SystemDataAccess = SystemDataAccessKind.None,
            IsDeterministic = true,
            IsPrecise = true)]
        public static int Distance(string value1, string value2)
        {
            int[] costs = new int[value1.Length + 1];

            // Add indexing for insertion to first row
            for (int i = 0; i < costs.Length; costs[i] = i++)
            {
            }

            int minSize = value1.Length < value2.Length ? value1.Length : value2.Length;

            for (int i = 0; i < minSize; i++)
            {
                costs[0] = i + 1;
                int addationCost = i;

                for (int j = 0; j < value1.Length; j++)
                {
                    int cost;
                    int insertionCost = costs[j + 1];

                    if (value2[i] == value1[j])
                    {
                        cost = addationCost;
                    }
                    else
                    {
                        cost = insertionCost < addationCost ?
                            insertionCost :         // insertion
                            addationCost;           // addation

                        cost = (costs[j] < cost ?
                            costs[j] :              // deletion
                            cost) + 1;
                    }

                    costs[j + 1] = cost;
                    addationCost = insertionCost;
                }
            }

            return costs[value1.Length];
        }
    }
}
