namespace Fastenshtein.Tests
{
    using Fastenshtein.Benchmarking;
    using System;
    using Xunit;

    public class ArrayFillTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(1024)]
        public void Repeated_Distance_Calls_Return_Correct_Distances(int length)
        {
            var onTest = new ArrayBenchmark();
            onTest.N = length;
            onTest.SetUp();

            var methods = new []
            { 
                onTest.SimpleLocal,
                onTest.SimpleUnroll,
                onTest.SimpleBackward,
                onTest.UnrollBackward,
                onTest.PointerSimple,
                onTest.PointerUnroll
            };

            foreach (var method in methods) 
            {
                var array = method();

                for (var i = 0; i < array.Length; i++)
                {
                    Assert.Equal(i, array[i]); 
                }
            }
        }
    }
}
