namespace SpeedTestApp.NuGetCompetitors.DuoVia
{
    internal class DuoViaFactory : ILevenshteinFactory
    {
        public string Name
        {
            get
            {
                return "DuoVia";
            }
        }

        public ILevenshtein Create(string value1)
        {
            return new DuoViaLevenshtein(value1);
        }
    }
}
