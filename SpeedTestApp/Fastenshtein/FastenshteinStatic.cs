namespace SpeedTestApp.Fastenshtein
{
    using global::Fastenshtein;

    internal class FastenshteinStatic : LevenshteinBase, ILevenshtein
    {
        public FastenshteinStatic(string value1)
            :base(value1)
        {
        }

        public int Distance(string value2)
        {
            return Levenshtein.Distance(this.Value1, value2);
        }
    }
}
