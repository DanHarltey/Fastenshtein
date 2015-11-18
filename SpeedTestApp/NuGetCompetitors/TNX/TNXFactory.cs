namespace SpeedTestApp.NuGetCompetitors.TNX
{
    using Fastenshtein;

    internal class TNXFactory : ILevenshteinFactory
    {
        public string Name
        {
            get
            {
                return "TNX.StringManipulation";
            }
        }

        public ILevenshtein Create(string value1)
        {
            return new TNXLevenshtein(value1);
        }
    }
}
