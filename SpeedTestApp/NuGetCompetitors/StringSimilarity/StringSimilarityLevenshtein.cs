namespace SpeedTestApp.NuGetCompetitors.StringSimilarity
{
    using F23.StringSimilarity;

    internal class StringSimilarityLevenshtein : LevenshteinBase, ILevenshtein
    {
        // I've read the source code it is thread safe
        private readonly static Levenshtein levenshtein = new Levenshtein();

        public StringSimilarityLevenshtein(string value1)
            : base(value1)
        {
        }

        public int Distance(string value2)
        {
            // why does it return a double
            var result = StringSimilarityLevenshtein.levenshtein.Distance(this.Value1, value2);

            return (int)result;
        }
    }
}