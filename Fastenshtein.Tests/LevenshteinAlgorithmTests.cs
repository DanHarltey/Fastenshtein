namespace Fastenshtein.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public abstract class LevenshteinAlgorithmTests
    {
        [TestMethod]
        public void Deletion_Adds_One_Distance_Test()
        {
            Test("test", "est", 1); // deletion
        }

        [TestMethod]
        public void Subsitation_Adds_One_Distance_Test()
        {
            Test("test", "tett", 1); // subsitation
        }

        [TestMethod]
        public void Addation_Adds_One_Distance_Test()
        {
            Test("test", "testt", 1); // addation
        }

        [TestMethod]
        public void Transposition_Adds_Two_Distance_Test()
        {
            Test("test", "tets", 2); // transposition
        }

        [TestMethod]
        public void Different_Case_Adds_One_Distance_Test()
        {
            Test("test", "Test", 1); // case 
        }

        [TestMethod]
        public void EmtpyString_Returns_Length_Test()
        {
            Test("test", string.Empty, 4);
            Test(string.Empty, "test", 4);
        }

        [TestMethod]
        public void EmtpyStrings_Returns_Zero_Test()
        {
            Test(string.Empty, string.Empty, 0);
        }

        protected abstract int CalculateDistance(string value1, string value2);

        private void Test(string value1, string value2, int expected)
        {
            int actual = this.CalculateDistance(value1, value2);
            Assert.AreEqual(expected, actual);
        }
    }
}
