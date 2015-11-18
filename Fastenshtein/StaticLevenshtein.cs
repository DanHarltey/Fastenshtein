namespace Fastenshtein
{
    using Microsoft.SqlServer.Server;

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
        [SqlFunction(
            Name= "LevenshteinDistance",
            DataAccess = DataAccessKind.None,
            SystemDataAccess = SystemDataAccessKind.None,
            IsDeterministic = true,
            IsPrecise = true)]
        public static int Distance(string value1, string value2)
        {
            int[] cost = new int[value2.Length + 1];

            // Add indexing for insertion to first row
            for (int i = 0; i < cost.Length; cost[i] = i++)
            {
            }

            for (int i = 0; i < value1.Length; i++)
            {
                cost[0] = i + 1;
                int addationCost = i;

                for (int j = 0; j < value2.Length; j++)
                {
                    int insertionCost = cost[j + 1];

                    if (value1[i] == value2[j])
                    {
                        cost[j + 1] = addationCost;
                    }
                    else
                    {
                        int tmp = insertionCost < addationCost ?
                            insertionCost :                         // insertion
                            addationCost;                           // addation

                        cost[j + 1] = (cost[j] < tmp ?
                            cost[j] :                               // deletion
                            tmp) + 1;
                    }

                    addationCost = insertionCost;
                }
            }

            return cost[value2.Length];
        }
    }
}
