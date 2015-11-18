namespace SpeedTestApp.NuGetCompetitors.NinjaNye
{
    using System;

    internal class NinjaNyeFactory : ILevenshteinFactory
    {
        public string Name
        {
            get
            {
                return "NinjaNye";
            }
        }

        public ILevenshtein Create(string value1)
        {
            return new NinjaNyeLevenshtein(value1);
        }
    }
}
