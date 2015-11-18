namespace SpeedTestApp.NuGetCompetitors.TNX
{
    internal class TNXLevenshtein : LevenshteinBase, ILevenshtein
    {

        public TNXLevenshtein(string value1)
            : base(value1)
        {
        }

        public int Distance(string value2)
        {
            return System.LevenshteinDistanceExtensions.LevenshteinDistanceFrom(this.Value1, value2);
        }
    }
}
