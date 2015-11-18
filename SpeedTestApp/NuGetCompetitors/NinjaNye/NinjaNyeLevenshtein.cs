namespace SpeedTestApp.NuGetCompetitors.NinjaNye
{
    using global::NinjaNye.SearchExtensions.Levenshtein;

    internal class NinjaNyeLevenshtein : LevenshteinBase, ILevenshtein
    {
        public NinjaNyeLevenshtein(string value1)
            : base(value1)
        {
        }

        public int Distance(string value2)
        {
            return LevenshteinProcessor.LevenshteinDistance(this.Value1, value2);
        }
    }
}
