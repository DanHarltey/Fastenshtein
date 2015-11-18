namespace SpeedTestApp.NuGetCompetitors.StringCompare
{
    internal class StringCompareLevenshtein : LevenshteinBase, ILevenshtein
    {

        public StringCompareLevenshtein(string value1)
            :base(value1)
        {
        }

        public int Distance(string value2)
        {
            var lev = new global::StringCompare.Algorithms.Levenshtein.LevenshteinAlgorithm();
            lev.GetCompareResult(this.Value1, value2);
            return 0;
        }
    }
}
