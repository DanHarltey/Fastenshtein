namespace Fastenshtein.Tests
{
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class LevenshteinEarlyExitTests
    {
        [Fact]
        public void Negative_MaxDistance_Returns_MinusOne()
        {
            Assert.Equal(-1, Levenshtein.Distance("test", "test", -1));
            Assert.Equal(-1, Levenshtein.Distance("test", "other", -1));

            var lev = new Levenshtein("test");
            Assert.Equal(-1, lev.DistanceFrom("test", -1));
            Assert.Equal(-1, lev.DistanceFrom("other", -1));

            Assert.Equal(-1, AutoCompleteLevenshtein.Distance("test", "test", -1));
        }

        [Fact]
        public void Zero_MaxDistance_ExactMatch_Returns_Zero()
        {
            Assert.Equal(0, Levenshtein.Distance("test", "test", 0));
            Assert.Equal(-1, Levenshtein.Distance("test", "tost", 0));

            var lev = new Levenshtein("test");
            Assert.Equal(0, lev.DistanceFrom("test", 0));
            Assert.Equal(-1, lev.DistanceFrom("tost", 0));

            Assert.Equal(0, AutoCompleteLevenshtein.Distance("test", "test ignored", 0));
            Assert.Equal(-1, AutoCompleteLevenshtein.Distance("test", "tost ignored", 0));
        }

        [Fact]
        public void Distance_At_Or_Below_MaxDistance_Returns_Exact_Distance()
        {
            // Substitutions (distance 1)
            Assert.Equal(1, Levenshtein.Distance("test", "tett", 1));
            Assert.Equal(1, Levenshtein.Distance("test", "tett", 2));

            // Deletions (distance 1)
            Assert.Equal(1, Levenshtein.Distance("test", "est", 1));

            // Insertions (distance 1)
            Assert.Equal(1, Levenshtein.Distance("test", "testt", 1));

            // Distance 2
            Assert.Equal(2, Levenshtein.Distance("test", "tets", 2));
            Assert.Equal(3, Levenshtein.Distance("kitten", "sitting", 3));
        }

        [Fact]
        public void Distance_Above_MaxDistance_Returns_MinusOne()
        {
            // Distance 2 but maxDistance 1
            Assert.Equal(-1, Levenshtein.Distance("test", "tets", 1));

            // Distance 3 but maxDistance 2
            Assert.Equal(-1, Levenshtein.Distance("kitten", "sitting", 2));

            // Completely different
            Assert.Equal(-1, Levenshtein.Distance("abcdef", "uvwxyz", 2));
        }

        [Fact]
        public void EmptyStrings_With_MaxDistance_Test()
        {
            Assert.Equal(0, Levenshtein.Distance(string.Empty, string.Empty, 0));
            Assert.Equal(0, Levenshtein.Distance(string.Empty, string.Empty, 2));

            Assert.Equal(3, Levenshtein.Distance("abc", string.Empty, 3));
            Assert.Equal(-1, Levenshtein.Distance("abc", string.Empty, 2));

            Assert.Equal(3, Levenshtein.Distance(string.Empty, "abc", 3));
            Assert.Equal(-1, Levenshtein.Distance(string.Empty, "abc", 2));

            var lev = new Levenshtein(string.Empty);
            Assert.Equal(0, lev.DistanceFrom(string.Empty, 0));
            Assert.Equal(3, lev.DistanceFrom("abc", 3));
            Assert.Equal(-1, lev.DistanceFrom("abc", 2));

            var levAbc = new Levenshtein("abc");
            Assert.Equal(3, levAbc.DistanceFrom(string.Empty, 3));
            Assert.Equal(-1, levAbc.DistanceFrom(string.Empty, 2));
        }

        [Fact]
        public void Instance_Reused_With_Varying_Thresholds()
        {
            var lev = new Levenshtein("kitten");

            Assert.Equal(0, lev.DistanceFrom("kitten", 0));
            Assert.Equal(1, lev.DistanceFrom("sitten", 1));
            Assert.Equal(-1, lev.DistanceFrom("sitting", 2));
            Assert.Equal(3, lev.DistanceFrom("sitting", 3));
            Assert.Equal(3, lev.DistanceFrom("sitting", 4));
            Assert.Equal(-1, lev.DistanceFrom("completelydifferent", 2));
            Assert.Equal(0, lev.DistanceFrom("kitten", 1));
        }

#if NET7_0_OR_GREATER
        [Fact]
        public void Span_EarlyExit_Matches_Static_Result()
        {
            ReadOnlySpan<char> s1 = "kitten".AsSpan();
            ReadOnlySpan<char> s2 = "sitting".AsSpan();

            Assert.Equal(3, Levenshtein.Distance(s1, s2, 3));
            Assert.Equal(-1, Levenshtein.Distance(s1, s2, 2));
            Assert.Equal(-1, Levenshtein.Distance(s1, s2, 0));
        }
#endif

        [Fact]
        public void AutoCompleteLevenshtein_EarlyExit_Test()
        {
            Assert.Equal(0, AutoCompleteLevenshtein.Distance("test", "test ignored", 0));
            Assert.Equal(0, AutoCompleteLevenshtein.Distance("test", "test ignored", 1));

            // Substitution in prefix
            Assert.Equal(1, AutoCompleteLevenshtein.Distance("test", "tast ignored", 1));
            Assert.Equal(-1, AutoCompleteLevenshtein.Distance("test", "tast ignored", 0));

            // 2 differences in prefix
            Assert.Equal(2, AutoCompleteLevenshtein.Distance("test", "tst ignored", 2));
            Assert.Equal(-1, AutoCompleteLevenshtein.Distance("test", "tst ignored", 1));

            // Empty strings
            Assert.Equal(0, AutoCompleteLevenshtein.Distance(string.Empty, "anything", 0));
            Assert.Equal(4, AutoCompleteLevenshtein.Distance("test", string.Empty, 4));
            Assert.Equal(-1, AutoCompleteLevenshtein.Distance("test", string.Empty, 3));
        }

        [Fact]
        public void Static_EarlyExit_IsThreadSafe()
        {
            string[] testData = RandomWords.Create(1000, 20);
            int[] expected = new int[testData.Length];
            int maxDistance = 3;

            for (int i = 0; i < testData.Length; i++)
            {
                int exact = Levenshtein.Distance(testData[0], testData[i]);
                expected[i] = exact <= maxDistance ? exact : -1;
            }

            Parallel.For(0, testData.Length, i =>
            {
                int actual = Levenshtein.Distance(testData[0], testData[i], maxDistance);
                Assert.Equal(expected[i], actual);
            });
        }

        [Fact]
        public void Fuzz_Testing_Against_Full_Distance()
        {
            var random = new Random(42);
            for (int iter = 0; iter < 5000; iter++)
            {
                int len1 = random.Next(0, 25);
                int len2 = random.Next(0, 25);

                char[] c1 = new char[len1];
                char[] c2 = new char[len2];
                for (int i = 0; i < len1; i++) c1[i] = (char)('a' + random.Next(0, 6));
                for (int i = 0; i < len2; i++) c2[i] = (char)('a' + random.Next(0, 6));

                string s1 = new string(c1);
                string s2 = new string(c2);

                int exact = Levenshtein.Distance(s1, s2);
                var lev = new Levenshtein(s1);

                for (int maxD = 0; maxD <= 5; maxD++)
                {
                    int expected = exact <= maxD ? exact : -1;

                    int staticRes = Levenshtein.Distance(s1, s2, maxD);
                    int instanceRes = lev.DistanceFrom(s2, maxD);

                    Assert.Equal(expected, staticRes);
                    Assert.Equal(expected, instanceRes);

#if NET7_0_OR_GREATER
                    int spanRes = Levenshtein.Distance(s1.AsSpan(), s2.AsSpan(), maxD);
                    Assert.Equal(expected, spanRes);
#endif
                }
            }
        }
    }
}

