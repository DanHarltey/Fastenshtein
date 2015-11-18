namespace SpeedTestApp.Fastenshtein
{
    using global::Fastenshtein;

    internal class FastenshteinLevenshtein : ILevenshtein
    {
        private readonly Levenshtein levenshtein;

        public FastenshteinLevenshtein(string value1)
        {
            this.levenshtein = new Levenshtein(value1);
        }

        public int Distance(string value2)
        {
            return this.levenshtein.Distance(value2);
        }
    }
}
