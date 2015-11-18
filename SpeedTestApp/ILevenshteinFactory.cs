namespace SpeedTestApp
{
    internal interface ILevenshteinFactory
    {
        string Name { get; }
        ILevenshtein Create(string value1);
    }
}
