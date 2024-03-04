////namespace Fastenshtein.Tests
////{
////    using Fastenshtein.Benchmarking;
////    using System.Linq;
////    using Xunit;

////    public class ArrayFillTests
////    {
////        [Theory]
////        [InlineData(0)]
////        [InlineData(1)]
////        [InlineData(4)]
////        [InlineData(8)]
////        [InlineData(9)]
////        [InlineData(1024)]
////        public void Repeated_Distance_Calls_Return_Correct_Distances(int length)
////        {
////            var onTest = new ArrayBenchmark();
////            onTest.N = length;

////            var methods = onTest
////                .GetType()
////                .GetMethods()
////                .Where(x => x.ReturnType == typeof(int[]));

////            foreach (var method in methods)
////            {
////                onTest.SetUp();

////                var array = (int[])method.Invoke(onTest, null);

////                for (var i = 0; i < array.Length; i++)
////                {
////                    Assert.Equal(i, array[i]);
////                }
////            }
////        }
////    }
////}
