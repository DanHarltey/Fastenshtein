namespace Fastenshtein.Benchmarking.FastenshteinOld
{
    public class Fastenshtein_1_0_0_5
    {
        private readonly string storedValue;
        private readonly int[] costs;

        /// <summary>
        /// Creates a new instance with a value to test other values against
        /// </summary>
        /// <param Name="value">Value to compare other values to.</param>
        public Fastenshtein_1_0_0_5(string value)
        {
            this.storedValue = value;
            // Create matrix row
            this.costs = new int[this.storedValue.Length];
        }

        /// <summary>
        /// gets the length of the stored value that is tested against
        /// </summary>
        public int StoredLength
        {
            get
            {
                return this.storedValue.Length;
            }
        }

        /// <summary>
        /// Compares a value to the stored value. 
        /// Not thread safe.
        /// </summary>
        /// <returns>Difference. 0 complete match.</returns>
        public int DistanceFrom(string value)
        {
            if (costs.Length == 0)
            {
                return value.Length;
            }

            // Add indexing for insertion to first row
            for (int i = 0; i < this.costs.Length;)
            {
                this.costs[i] = ++i;
            }

            for (int i = 0; i < value.Length; i++)
            {
                // cost of the first index
                int cost = i;
                int addationCost = i;

                // cache value for inner loop to avoid index lookup and bonds checking, profiled this is quicker
                char value1Char = value[i];

                for (int j = 0; j < this.storedValue.Length; j++)
                {
                    int insertionCost = cost;

                    cost = addationCost;

                    // assigning this here reduces the array reads we do, improvment of the old version
                    addationCost = this.costs[j];

                    if (value1Char != this.storedValue[j])
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

                    this.costs[j] = cost;
                }
            }

            return this.costs[this.costs.Length - 1];
        }

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
                int addationCost = i;

                // cache value for inner loop to avoid index lookup and bonds checking, profiled this is quicker
                char value1Char = value1[i];

                for (int j = 0; j < value2.Length; j++)
                {
                    int insertionCost = cost;

                    cost = addationCost;

                    // assigning this here reduces the array reads we do, improvment of the old version
                    addationCost = costs[j];

                    if (value1Char != value2[j])
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