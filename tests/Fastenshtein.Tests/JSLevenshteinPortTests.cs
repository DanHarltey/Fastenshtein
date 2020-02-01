using Fastenshtein.Benchmarking;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Fastenshtein.Tests
{
    public class JSLevenshteinPortTests : LevenshteinTests
    {
        protected override int CalculateDistance(string value1, string value2)
        {
            var lev = new JSLevenshteinPort(value1);
            return lev.DistanceFrom(value2);
        }
    }
}
