namespace MessNTest;

using System;
using System.Text;

public static class Program
{
    static void Main()
    {
        ////var xWord = "dave"; 
        ////var yWord = "yy";
        //var xWord = "xxxx";
        //var yWord = "yyyy";
        var xWord = "xx";
        var yWord = "xx";

        var grid = CalculateGrid(xWord, yWord);
        var square = PrintGrid(xWord, yWord, grid);
        Console.Write(square);

        var dGrid = DGrid(xWord, yWord, grid);
        Console.Write(dGrid);

        var dGridWithRows = DGridWithRowsWorking(xWord, yWord, grid);
        Console.Write(dGridWithRows);
        //DGrid(grid);

    }

    public static int DGridWithRows(string xWord, string yWord)
    {
        if (xWord.Length == 0)
        {
            return yWord.Length;
        }
        
        var previous = new int[xWord.Length + 1];
        var current = new int[xWord.Length + 1];

        for (int i = 0; i < previous.Length; i++)
        {
            previous[i] = i;
            current[i] = i;
        }

        var startY = 0;
        var startX = 1;
        var max = xWord.Length + yWord.Length;

        for (int i = 1; i < max; ++i)
        {
            previous[0] = i;

            var y = startY;
            var x = startX;

            ///var otherY = (i - startX);
            //var xEnd =   
            //    if (i > yWord.Length)
            //{
            //    xMin = i - yWord.Length;
            //}
            //else
            //{
            //    xMin = 1;
            //}

            // could be for loop with x min/max
            while (x > 0 && y < yWord.Length)
            {
                var cost = (xWord[x - 1] == yWord[y]) ? 0 : 1;

                var value = Math.Min(
                    current[x - 1] + cost,
                    Math.Min(previous[x] + 1, previous[x - 1] + 1));

                current[x] = value;

                y++;
                x--;
            }

            if (startX < xWord.Length)
            {
                startX++;
            }
            else
            {
                startY++;
                // could not store this and just start bumming xMin
            }

            var tmp = previous;
            previous = current;
            current = tmp;
        }

        return previous[previous.Length - 1];
    }

    public static string DGridWithRowsWorking(string xWord, string yWord, int[][] matrix)
    {
        var xLen = xWord.Length + 1;
        var yLen = yWord.Length + 1;

        StringBuilder sb = new();

        // header
        sb.Append("  ");
        sb.AppendLine(xWord);
        sb.Append(' ');

        for (int i = 0; i < xLen; i++)
        {
            sb.Append(matrix[0][i]);
        }
        sb.AppendLine();

        var startY = 1;
        var startX = 1;

        var previous = new int[xWord.Length + 1];
        var current = new int[xWord.Length + 1];

        for (int i = 0; i < previous.Length; i++)
        {
            previous[i] = i;
            current[i] = i;
        }

        var max = xWord.Length + yWord.Length - 1;

        for (int i = 0; i < max; ++i)
        {
            previous[0] = i + 1;
            //current[0] = i + 1;

            var x = startX;

            if (i < yWord.Length)
            {
                sb.Append(yWord[i]);
                sb.Append(matrix[i][0]);
            }
            else
            {
                sb.Append("  ");
            }

            sb.Append(string.Concat(Enumerable.Repeat(' ', startY - 1)));

            var y = startY;
            while (x > 0 && y < yLen)
            {
                var cost = (xWord[x - 1] == yWord[y - 1]) ? 0 : 1;

                var localCost = Math.Min(previous[x - 1], previous[x]);

                var maddd = GetValue(previous, current, xWord, yWord, x, y);

                var newValue = Math.Min(
                    current[x - 1] + cost,
                    Math.Min(previous[x] + 1, previous[x - 1] + 1));
                //var newValue = Math.Min(
                //    previous[x - 1] + cost,
                //    Math.Min(previous[x] + 1, current[x - 1] + 1));

                ////            var cost = (xWord[x - 1] == yWord[y - 1]) ? 0 : 1;
                ////                matrix[y][x] = Math.Min(
                ////                    Math.Min(matrix[y - 1][x] + 1, matrix[y][x - 1] + 1),
                ////                    matrix[y - 1][x - 1] + cost);

                var oldValue = matrix[y][x];


                {
                    Console.WriteLine($"x: {x} y: {y}");

                    //Console.WriteLine("old");
                    //Console.WriteLine($"{matrix[y - 1][x - 1]} {matrix[y - 1][x]}");
                    //Console.WriteLine($"{matrix[y][x - 1]} {oldValue}");

                    //Console.WriteLine("new");
                    //Console.WriteLine($"{previous[x - 1]} {previous[x]}");
                    //Console.WriteLine($"{current[x - 1]} {newValue}");

                    Console.WriteLine("Before");
                    Console.WriteLine(String.Join("|", previous.Select(x => x.ToString())));
                    Console.WriteLine(String.Join("|", current.Select(x => x.ToString())));

                    current[x] = newValue;
                    Console.WriteLine("After");
                    Console.WriteLine(String.Join("|", previous.Select(x => x.ToString())));
                    Console.WriteLine(String.Join("|", current.Select(x => x.ToString())));
                }

                sb.Append(newValue.ToString());
                ////sb.Append(oldValue.ToString());
                if (oldValue != newValue)
                {

                }
                y++;
                x--;

                Console.WriteLine();
                sb.AppendLine();
            }
            ////sb.AppendLine();

            if (startX < xWord.Length)
            {
                startX++;
            }
            else
            {
                startY++;
            }

            var tmp = previous;
            previous = current;
            current = tmp;
        }

        return sb.ToString();
    }

    private static int GetValue(int[] previous, int[] current, string xWord, string yWord, int x, int y)
    {
        int value = 0;
        var localCost = Math.Min(previous[x], previous[x - 1]);

        if (localCost < current[x - 1])
        {
            value = localCost + 1;
        }
        else
        {
            int cost;
            if (xWord[x - 1] != yWord[y - 1])
            {
                cost = 1;
            }
            else
            {
                cost = 0;
            }

            value = current[x - 1] + cost;
        }

        return value;
    }

    public static string DGrid(string xWord, string yWord, int[][] matrix)
    {
        var xLen = xWord.Length + 1;
        var yLen = yWord.Length + 1;

        StringBuilder sb = new();

        // header
        sb.Append("  ");
        sb.AppendLine(xWord);
        sb.Append(' ');

        for (int i = 0; i < xLen; i++)
        {
            sb.Append(matrix[0][i]);
        }
        sb.AppendLine();

        var startY = 1;
        var startX = 1;

        var max = xWord.Length + yWord.Length - 1;

        for (int i = 0; i < max; ++i)
        {
            var x = startX;

            if (i < yWord.Length)
            {
                sb.Append(yWord[i]);
                sb.Append(matrix[i][0]);
            }
            else
            {
                sb.Append("  ");
            }

            sb.Append(string.Concat(Enumerable.Repeat(' ', startY - 1)));

            var y = startY;
            while (x > 0 && y < yLen)
            {
                sb.Append(matrix[y++][x--].ToString());
            }
            sb.AppendLine();

            if (startX < xWord.Length)
            {
                startX++;
            }
            else
            {
                startY++;
            }
        }

        return sb.ToString();
    }

    public static string DGridWrongWay(string xWord, string yWord, int[][] matrix)
    {
        var xLen = xWord.Length + 1;

        StringBuilder sb = new();

        // header
        sb.Append("  ");
        sb.AppendLine(xWord);
        sb.Append(' ');

        for (int i = 0; i < xLen; i++)
        {
            sb.Append(matrix[0][i]);
        }
        sb.AppendLine();

        var startY = 1;
        var startX = 1;

        var max = xWord.Length + yWord.Length - 1;

        for (int i = 0; i < max; ++i)
        {
            var x = startX;

            if (x == 1)
            {
                sb.Append(yWord[startY - 1]);
                sb.Append(matrix[startY][0]);
            }
            else
            {
                sb.Append("  ");
            }

            sb.Append(string.Concat(Enumerable.Repeat(' ', x - 1)));

            var y = startY;
            while (x < xLen && y >= 1)
            {
                sb.Append(matrix[y--][x++].ToString());
            }
            sb.AppendLine();

            if (startY < yWord.Length)
            {
                startY++;
            }
            else
            {
                startX++;
            }
        }

        return sb.ToString();
    }

    public static string DGrid2(string xWord, string yWord, int[][] matrix)
    {
        var xLen = xWord.Length + 1;
        var yLen = yWord.Length + 1;

        StringBuilder sb = new();

        // header
        sb.Append("  ");
        sb.AppendLine(xWord);
        sb.Append(' ');

        for (int i = 0; i < xLen; i++)
        {
            sb.Append(matrix[0][i]);
        }
        sb.AppendLine();

        var startY = 1;
        var minY = 1;

        var startX = 1;


        var count = 0;
        while (startX < xLen)
        {
            ++count;
            var x = startX;

            if (x == 1)
            {
                sb.Append(yWord[startY - 1]);
                sb.Append(matrix[startY][0]);
            }
            else
            {
                sb.Append("  ");
            }

            sb.Append(string.Concat(Enumerable.Repeat(' ', x - 1)));

            var y = startY;
            while (x < xLen && y >= minY)
            {
                sb.Append(matrix[y--][x++].ToString());
            }
            sb.AppendLine();

            if (startY < yLen - 1)
            {
                startY++;
            }
            else
            {
                startX++;
            }
        }

        sb.AppendLine(count.ToString());
        return sb.ToString();
    }

    public static string DGridOg(string xWord, string yWord, int[][] matrix)
    {
        var xLen = xWord.Length + 1;
        var yLen = yWord.Length + 1;

        StringBuilder sb = new();

        // header
        sb.Append("  ");
        sb.AppendLine(xWord);
        sb.Append(' ');

        for (int i = 0; i < xLen; i++)
        {
            sb.Append(matrix[0][i]);
        }
        sb.AppendLine();

        var maxY = 1;
        var minY = 0;
        var minX = 1;
        while (minX <= xLen)
        //for (var i = 0; i < yLen + xLen; i++)
        {
            //if (minY == yLen)
            //    break;

            var x = minX;

            if (x == 1)
            {
                sb.Append(yWord[maxY - 1]);
                sb.Append(matrix[maxY][0]);
            }
            else
            {
                sb.Append("  ");
            }


            sb.Append(string.Concat(Enumerable.Repeat(' ', x - 1)));
            for (int y = maxY; y > minY; y--)
            {
                if (x > xLen - 1)
                {
                    break;
                }
                sb.Append(matrix[y][x++].ToString());
            }
            sb.AppendLine();

            if (maxY < yLen - 1)
            {
                maxY++;
            }
            else
            {
                minX++;

                if (minX + (yLen - maxY) >= xLen)
                {
                    minY++;
                }
            }
            //for (var x = 0; x < y; x++)
            //{
            //    Console.WriteLine(grid[x, y]);
            //}
            //var loops = 2;// xLen + yLen;

            //var x = 1;
            //var y = 1;
            //var xStart = 1;
            //var xCount = 1;

            //Console.WriteLine(grid[x, y]);

            //for (int i = 0; i < loops; i++)
            //{

            //}
        }

        return sb.ToString();
    }

    public static int[][] CalculateGrid(string xWord, string yWord)
    {
        var source1Length = yWord.Length;
        var source2Length = xWord.Length;

        var matrix = new int[source1Length + 1][];

        ////// First calculation, if one entry is empty return full length
        ////if (source1Length == 0)
        ////    return matrix;

        ////if (source2Length == 0)
        ////    return matrix;

        // Initialization of matrix with row size source1Length and columns size source2Length
        for (var i = 0; i <= source1Length;)
        {
            matrix[i] = new int[source2Length + 1];
            matrix[i][0] = i++;
        }

        for (var j = 0; j <= source2Length; matrix[0][j] = j++)
        {
        }

        // Calculate rows and collumns distances
        for (var y = 1; y <= source1Length; y++)
        {
            for (var x = 1; x <= source2Length; x++)
            {
                var cost = (xWord[x - 1] == yWord[y - 1]) ? 0 : 1;

                matrix[y][x] = Math.Min(
                    Math.Min(matrix[y - 1][x] + 1, matrix[y][x - 1] + 1),
                    matrix[y - 1][x - 1] + cost);
            }
        }

        // return result
        return matrix;
    }

    public static string PrintGrid(string xWord, string yWord, int[][] matrix)
    {
        var sb = new StringBuilder();

        sb.Append("  ");
        sb.AppendLine(xWord);

        for (var y = 0; y <= yWord.Length; y++)
        {
            if (y == 0)
            {
                sb.Append(' ');
            }
            else
            {
                sb.Append(yWord[y - 1]);
            }

            for (var x = 0; x <= xWord.Length; x++)
            {
                sb.Append(matrix[y][x]);
            }
            sb.AppendLine();
        }

        return sb.ToString();
    }
}