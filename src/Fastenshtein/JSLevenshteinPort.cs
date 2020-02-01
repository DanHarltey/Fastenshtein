using System.Runtime.CompilerServices;

namespace Fastenshtein
{
    /// <summary>
    /// Measures the difference between two strings.
    /// Uses the Levenshtein string difference algorithm.
    /// </summary>
    public partial class JSLevenshteinPort
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
        public JSLevenshteinPort(string value)
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

            int x = 0;
            int y;
            int d0;
            int d1;
            int d2;
            int d3;
            int dd = costs.Length;
            int dy;
            char ay;
            char bx0;
            char bx1;
            char bx2;
            char bx3;

            for (; x < value.Length - 3;)
            {
                bx0 = value[d0 = x];
                bx1 = value[d1 = x + 1];
                bx2 = value[d2 = x + 2];
                bx3 = value[d3 = x + 3];
                dd = (x += 4);

                for (y = 0; y < this.storedValue.Length; y++)
                {
                    dy = this.costs[y];
                    ay = this.storedValue[y];
                    d0 = Min(dy, d0, d1, bx0, ay);
                    d1 = Min(d0, d1, d2, bx1, ay);
                    d2 = Min(d1, d2, d3, bx2, ay);
                    dd = Min(d2, d3, dd, bx3, ay);
                    this.costs[y] = dd;
                    d3 = d2;
                    d2 = d1;
                    d1 = d0;
                    d0 = dy;
                }
            }

            for (; x < value.Length;)
            {
                bx0 = value[d0 = x];
                dd = ++x;
                for (y = 0; y < this.storedValue.Length; y++)
                {
                    dy = this.costs[y];
                    this.costs[y] = dd = Min(dy, d0, dd, bx0, this.storedValue[y]);
                    d0 = dy;
                }
            }

            return dd;
        }

#if NETSTANDARD1_0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static int Min(int d0, int d1, int d2, char bx, char ay)
        {
            int result;

            if (d0 < d1 || d2 < d1)
            {
                if (d0 > d2)
                {
                    result = d2 + 1;
                }
                else
                {
                    result = d0 + 1;
                }
            }
            else if (bx == ay)
            {
                result = d1;
            }
            else
            {
                result = d1 + 1;
            }

            return result;
        }
    }
}