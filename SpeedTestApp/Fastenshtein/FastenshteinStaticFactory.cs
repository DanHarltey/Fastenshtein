namespace SpeedTestApp.Fastenshtein
{
    internal class FastenshteinStaticFactory : ILevenshteinFactory
    {
        public string Name
        {
            get
            {
                return "FastenshteinStatic";
            }
        }

        public ILevenshtein Create(string value1)
        {
            return new FastenshteinStatic(value1);
        }
    }
}
