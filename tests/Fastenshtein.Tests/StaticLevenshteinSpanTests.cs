#if NET7_0_OR_GREATER
namespace Fastenshtein.Tests;

using System;

public class StaticLevenshteinSpanTests : StaticLevenshteinTests
{
    protected override int CalculateDistance(string value1, string value2)
        => Levenshtein.Distance(value1.AsSpan(), value2.AsSpan());
}
#endif