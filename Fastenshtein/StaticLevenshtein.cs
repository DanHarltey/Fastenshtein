namespace Fastenshtein
{
#if !PCL
    using Microsoft.SqlServer.Server;
#endif

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
#if !PCL
        [SqlFunction(
            Name= "LevenshteinDistance",
            DataAccess = DataAccessKind.None,
            SystemDataAccess = SystemDataAccessKind.None,
            IsDeterministic = true,
            IsPrecise = true)]
#endif
        public static int Distance(string value1, string value2)
        {
            // Check if both values are identical first
            if (value1 == value2) return 0;

            int[] costs = new int[value2.Length + 1];

            // Add indexing for insertion to first row
            for (int i = 0; i < costs.Length; costs[i] = i++)
            {
            }

            for (int i = 0; i < value1.Length; i++)
            {
                costs[0] = i + 1;
                int addationCost = i;

                for (int j = 0; j < value2.Length; j++)
                {
                    int cost;
                    int insertionCost = costs[j + 1];

                    if (value1[i] == value2[j])
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

            return costs[value2.Length];
        }
    }
}
