namespace SpeedTestApp.NuGetCompetitors.DuoVia
{ 
    internal class DuoViaLevenshtein : LevenshteinBase, ILevenshtein
    {
        public DuoViaLevenshtein(string value1)
            : base(value1)
        {
        }

        public int Distance(string value2)
        {
            return global::DuoVia.FuzzyStrings.LevenshteinDistanceExtensions.LevenshteinDistance(
                this.Value1,
                value2,
                false);
        }
    }
}
