namespace SpeedTestApp.Fastenshtein
{
    internal class FastenshteinFactory : ILevenshteinFactory
    {
        public string Name
        {
            get
            {
                return "Fastenshtein";
            }
        }

        public ILevenshtein Create(string value1)
        {
            return new FastenshteinLevenshtein(value1);
        }
    }
}
