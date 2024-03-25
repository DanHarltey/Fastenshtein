#if NET6_0_OR_GREATER
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endif

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
            this.costs = new int[this.storedValue.Length];
        }

        /// <summary>
        /// gets the length of the stored value that is tested against
        /// </summary>
        public int StoredLength => this.storedValue.Length;


#if NET6_0_OR_GREATER

        /// <summary>
        /// Compares a value to the stored value. 
        /// Not thread safe.
        /// </summary>
        /// <returns>Difference. 0 complete match.</returns>
        public int DistanceFrom3(string value)
        {
            var costs = this.costs;
            var storedValue = this.storedValue;

            if (costs.Length == 0 || costs.Length != storedValue.Length)
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
                    cost = costs[j];// Unsafe.Add(ref refCosts, j);

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
        /// Compares a value to the stored value. 
        /// Not thread safe.
        /// </summary>
        /// <returns>Difference. 0 complete match.</returns>
        public int DistanceFrom2(string value)
        {
            var costs = this.costs;
            ref var refCosts = ref MemoryMarshal.GetArrayDataReference(this.costs);

            if (costs.Length == 0)
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

                for (int j = 0; j < this.storedValue.Length; j++)
                {
                    int currentCost = cost;

                    // assigning this here reduces the array reads we do, improvement of the old version
                    cost = Unsafe.Add(ref refCosts, j);

                    if (value1Char != this.storedValue[j])
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
                    Unsafe.Add(ref refCosts, j) = currentCost;
                    previousCost = currentCost;
                }
            }

            return costs[costs.Length - 1];
        }

        /// <summary>
        /// Compares a value to the stored value. 
        /// Not thread safe.
        /// </summary>
        /// <returns>Difference. 0 complete match.</returns>
        public int DistanceFrom4(string value)
        {
            var costs = this.costs;

            if (costs.Length == 0)
            {
                return value.Length;
            }

            // Add indexing for insertion to first row
            ref var refCosts = ref MemoryMarshal.GetArrayDataReference(costs);
            for (int i = 1; i <= costs.Length; i++)
            {
                refCosts = i;
                refCosts = ref Unsafe.Add(ref refCosts, 1);
            }

            for (int i = 0; i < value.Length; i++)
            {
                // cost of the first index
                int cost = i;
                int previousCost = i;

                // cache value for inner loop to avoid index lookup and bonds checking, profiled this is quicker
                char value1Char = value[i];

                for (int j = 0; j < this.storedValue.Length; j++)
                {
                    int currentCost = cost;

                    // assigning this here reduces the array reads we do, improvement of the old version
                    cost = costs[j];

                    if (value1Char != this.storedValue[j])
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
        /// Compares a value to the stored value. 
        /// Not thread safe.
        /// </summary>
        /// <returns>Difference. 0 complete match.</returns>
        public int DistanceFrom5(string value)
        {
            var costs = this.costs;
            var storedValue = this.storedValue;

            if (costs.Length == 0 || costs.Length != storedValue.Length)
            {
                return value.Length;
            }

            int previousCost = 0;

            // Add indexing for insertion to first row
            for (; previousCost < costs.Length;)
            {
                costs[previousCost] = ++previousCost;
            }

            for (int i = 0; i < value.Length; i++)
            {
                // cost of the first index
                int cost = i;
                previousCost = i;

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

            return previousCost;
        }

        /// <summary>
        /// Compares a value to the stored value. 
        /// Not thread safe.
        /// </summary>
        /// <returns>Difference. 0 complete match.</returns>
        public int DistanceFrom(string value)
        {
            var costs = this.costs;
            var storedValue = this.storedValue;
            ref var storedValueRef = ref MemoryMarshal.GetReference(storedValue.AsSpan());

            if (costs.Length == 0)
            {
                return value.Length;
            }

            int previousCost = 0;
            ref var refCosts = ref MemoryMarshal.GetArrayDataReference(costs);
            for (; previousCost < costs.Length;)
            {
                refCosts = ++previousCost;
                refCosts = ref Unsafe.Add(ref refCosts, 1);
            }
            refCosts = ref MemoryMarshal.GetArrayDataReference(costs);

            for (int i = 0; i < value.Length; i++)
            {
                // cost of the first index
                int cost = i;
                previousCost = i;

                // cache value for inner loop to avoid index lookup and bonds checking, profiled this is quicker
                char value1Char = value[i];

                for (int j = 0; j < storedValue.Length; j++)
                {
                    int currentCost = cost;

                    // assigning this here reduces the array reads we do, improvement of the old version
                    cost = Unsafe.Add(ref refCosts, j);

                    if (value1Char != Unsafe.Add(ref storedValueRef, j))
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
                    Unsafe.Add(ref refCosts, j) = currentCost;
                    previousCost = currentCost;
                }
            }

            return previousCost;
        }

#else
        /// <summary>
        /// Compares a value to the stored value. 
        /// Not thread safe.
        /// </summary>
        /// <returns>Difference. 0 complete match.</returns>
        public int DistanceFrom(string value)
        {
            var costs = this.costs;

            if (costs.Length == 0)
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

                for (int j = 0; j < this.storedValue.Length; j++)
                {
                    int currentCost = cost;

                    // assigning this here reduces the array reads we do, improvement of the old version
                    cost = costs[j];

                    if (value1Char != this.storedValue[j])
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
#endif
    }

}
