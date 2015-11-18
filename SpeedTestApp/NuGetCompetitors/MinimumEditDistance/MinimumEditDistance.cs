
namespace SpeedTestApp.NuGetCompetitors.MinimumEditDistance
{
    using MinimumEditDistance;

    internal class MinimumEditDistanceLevenshtein : LevenshteinBase, ILevenshtein
    {
        public MinimumEditDistanceLevenshtein(string value1)
            :base(value1)
        {
        }

        public int Distance(string value2)
        {
            return global::MinimumEditDistance.Levenshtein.CalculateDistance(this.Value1, value2, 1);
        }
    }
}