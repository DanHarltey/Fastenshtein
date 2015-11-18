namespace SpeedTestApp.NuGetCompetitors.StringCompare
{
    using System;
    using SpeedTestApp.Fastenshtein;

    internal class StringCompareFactory : ILevenshteinFactory
    {
        public string Name
        {
            get
            {
                return "StringCompare";
            }
        }

        public ILevenshtein Create(string value1)
        {
            return new StringCompareLevenshtein(value1);
        }
    }
}
