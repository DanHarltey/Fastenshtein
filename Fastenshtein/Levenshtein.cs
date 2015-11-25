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

        private readonly string storedValue;
        private readonly int[] costs;

        /// <summary>
        /// Creates a new instance with a value to test other values against
        /// </summary>
        /// <param Name="value">Value to compare other values to.</param>
        public Levenshtein(string value)
        {
            this.storedValue = value;
            // Create matrix row
            this.costs = new int[this.storedValue.Length + 1];
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
        public int Distance(string value)
        {
            // Add indexing for insertion to first row
            for (int i = 0; i < this.costs.Length; this.costs[i] = i++)
            {
            }

            for (int i = 0; i < value.Length; i++)
            {
                this.costs[0] = i + 1;
                int addationCost = i;

                for (int j = 0; j < this.storedValue.Length; j++)
                {
                    int cost;
                    int insertionCost = this.costs[j + 1];

                    if (value[i] == this.storedValue[j])
                    {
                        cost = addationCost;
                    }
                    else
                    {
                        cost = insertionCost < addationCost ?
                            insertionCost :                         // insertion
                            addationCost;                           // addation

                        cost = (this.costs[j] < cost ?
                            this.costs[j] :                         // deletion
                            cost) + 1;
                    }

                    this.costs[j + 1] = cost;
                    addationCost = insertionCost;
                }
            }

            return this.costs[this.storedValue.Length];
        }
    }
}
