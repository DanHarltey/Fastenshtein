namespace MessNTest.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void SquareGrid()
        {
            var xWord = "xxx";
            var yWord = "yyy";

            var grid = Program.CalculateGrid(xWord, yWord);
            var actual = Program.PrintGrid(xWord, yWord, grid);


            var expected = @"  xxx
 0123
y1123
y2223
y3333
";
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void SquareDGrid()
        {
            var xWord = "xxx";
            var yWord = "yyy";

            var grid = Program.CalculateGrid(xWord, yWord);
            var actual = Program.DGrid(xWord, yWord, grid);

            var expected = @"  xxx
 0123
y01
y122
y2323
   33
    3
";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SquareLongGrid()
        {
            var xWord = "xxxx";
            var yWord = "yy";

            var grid = Program.CalculateGrid(xWord, yWord);
            var actual = Program.PrintGrid(xWord, yWord, grid);


            var expected = @"  xxxx
 01234
y11234
y22234
";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SquareLongDGrid()
        {
            var xWord = "xxxx";
            var yWord = "yy";

            var grid = Program.CalculateGrid(xWord, yWord);
            var actual = Program.DGrid(xWord, yWord, grid);


            var expected = @"  xxxx
 01234
y01
y122
  32
  43
   4
";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SquarSortGrid()
        {
            var xWord = "xx";
            var yWord = "yyyy";

            var grid = Program.CalculateGrid(xWord, yWord);
            var actual = Program.PrintGrid(xWord, yWord, grid);

            var expected = @"  xx
 012
y112
y222
y333
y444
";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SquarSortDGrid()
        {
            var xWord = "xx";
            var yWord = "yyyy";

            var grid = Program.CalculateGrid(xWord, yWord);
            var actual = Program.DGrid(xWord, yWord, grid);

            var expected = @"  xx
 012
y01
y122
y2 23
y3  34
     4
";
            Assert.Equal(expected, actual);
        }
    }
}
