namespace SpeedTestApp.NuGetCompetitors.MinimumEditDistance
{
    using Fastenshtein;

    internal class MinimumEditDistanceFactory : ILevenshteinFactory
    {
        public string Name
        {
            get
            {
                return "MinimumEditDistance";
            }
        }

        public ILevenshtein Create(string value1)
        {
            return new MinimumEditDistanceLevenshtein(value1);
        }
    }
}