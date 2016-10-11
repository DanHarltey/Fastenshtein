namespace SpeedTestApp.NuGetCompetitors.StringSimilarity
{
    internal class StringSimilarityFactory : ILevenshteinFactory
    {
        public string Name
        {
            get
            {
                return "StringSimilarity.NET";
            }
        }

        public ILevenshtein Create(string value1)
        {
            return new StringSimilarityLevenshtein(value1);
        }
    }
}
