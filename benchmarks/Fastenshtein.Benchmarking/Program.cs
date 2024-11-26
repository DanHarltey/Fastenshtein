namespace Fastenshtein.Benchmarking;

using System;
using System.Net.WebSockets;

static class Program
{
    /*
     * If you want to add your method to the benchmarking. 
     * There are just two files that need altering 
     * BenchmarkMultiThread & BenchmarkSingleThread.
     */

    static void Main(string[] args)
    {
        DateTime startTime = DateTime.UtcNow;

        ////if (args.Length != 0 && string.Equals(args[0], "d", StringComparison.OrdinalIgnoreCase))
        ////{
        ////    _ = BenchmarkRunner.Run<FastenshteinDisassembly>();
        ////}
        ////else if (args.Length != 0 && string.Equals(args[0], "c", StringComparison.OrdinalIgnoreCase))
        ////{
        ////    _ = BenchmarkRunner.Run<CompetitiveBenchmarkSmallWordsSingleThread>();
        ////    _ = BenchmarkRunner.Run<CompetitiveBenchmarkNormalWordsSingleThread>();
        ////    _ = BenchmarkRunner.Run<CompetitiveBenchmarkLargeWordsSingleThread>();

        ////    _ = BenchmarkRunner.Run<CompetitiveBenchmarkSmallWordsMultiThread>();
        ////    _ = BenchmarkRunner.Run<CompetitiveBenchmarkNormalWordsMultiThread>();
        ////    _ = BenchmarkRunner.Run<CompetitiveBenchmarkLargeWordsMultiThread>();
        ////}
        ////else
        ////{
        ////    _ = BenchmarkRunner.Run<BenchmarkSmallWordsSingleThread>();
        ////    _ = BenchmarkRunner.Run<BenchmarkNormalWordsSingleThread>();
        ////    _ = BenchmarkRunner.Run<BenchmarkLargeWordsSingleThread>();
        ////}

        //var value1 = "test";
        var xValue = "ab";
        var yValue = "ab";

        /*
         * 0,0
         * 
         *   0, 1
         * 0 0
         * 
         * 1,0
         * 
         * 
         *   0 1
         * 0 0 1
         * 
        /*
         * 
            0,0 :
            1,0 : 0,1 :
            1,1 : 0,2 :
            1,2 : 0,3 :
            1,3 :

            0,0 :
            1,0 : 0,1 :
            2,0 : 1,1 : 0,2 :
            3,0 : 2,1 : 1,2 : 0,3 :
            3,1 : 2,2 : 1,3 :
            3,2 : 2,3 :
            3,3 :

  t e
t x y
e y       
        */
        //Loop3(xValue, yValue);
        //Console.WriteLine();
        ////LoopXFirst(xValue, yValue);
        var result2 = Levenshtein.DistanceDualRow(yValue, xValue);
        Console.WriteLine("new");
        var result = 0;// Levenshtein.Distance(yValue, xValue);

        if(result != result2)
        {
            Console.WriteLine("not match");
        }
        //LoopXFirst(xValue, yValue);

        Console.WriteLine();
        Console.WriteLine();


        //Loop(xValue, yValue);


        Console.WriteLine("Completed in : " + (DateTime.UtcNow - startTime));

    }

    private static int LoopXFirst(string xValue, string yValue)
    {
        var result = 0;

        var loops = xValue.Length + yValue.Length - 1;
        var minX = 0;
        var maxX = 0;

        for (int i = 0; i < loops; i++)
        {
            if (i >= yValue.Length)
            {
                minX++;
            }

            if (i < xValue.Length)
            {
                maxX++;
            }

            var y = i - minX;
            for (int x = minX; x < maxX; x++)
            {
                result ^= xValue[x] ^ yValue[y];
                Console.Write($"{x},{y} : ");
                --y;
            }
            Console.WriteLine();
        }

        return result;
    }

    private static int Loop3(string xValue, string yValue)
    {
        var result = 0;


        for (int counter = 1; counter <= xValue.Length + yValue.Length; counter++)
        {
            int startRow;


            if (counter > xValue.Length)
            {
                startRow = counter - yValue.Length;
            }
            else
            {
                startRow = 1;
            }

            int endRow;
            if (counter > xValue.Length)
            {
                endRow = yValue.Length;
            }
            else
            {
                endRow = counter - 1;
            }

            for (int y = endRow; y >= startRow;)
            {
                int x = counter - y;

                result ^= xValue[x - 1] ^ yValue[y - 1];
                Console.Write($"{x - 1},{y - 1} : ");

                y--;
            }

            Console.WriteLine();
        }

        ////    var x = i - minY;
        ////    for (int y = minY; y < maxY; y++)
        ////    {
        ////        //int x = (i - y);
        ////        result ^= xValue[x] ^ yValue[y];
        ////        Console.Write($"{x},{y} : ");
        ////        --x;
        ////    }
        ////    Console.WriteLine();
        ////}

        return result;
    }

    private static int Loop2_2(string xValue, string yValue)
    {
        var result = 0;

        var loops = xValue.Length + yValue.Length - 1;
        var minY = 0;
        var maxY = 0;

        for (int i = 0; i < loops; i++)
        {
            if (i >= xValue.Length)
            {
                minY++;
            }

            if (i < yValue.Length)
            {
                maxY++;
            }

            var x = i - minY;
            for (int y = minY; y < maxY; y++)
            {
                if (yValue[y] != xValue[x])
                {
                    Console.Write($"{yValue[y]} {xValue[x]}");
                }
                else
                {
                    Console.WriteLine("same");
                }

                --x;
            }
            Console.WriteLine();
        }

        return result;
    }

    private static int Loop2(string xValue, string yValue)
    {
        var result = 0;

        var loops = xValue.Length + yValue.Length - 1;
        var minY = 0;
        var maxY = 0;

        for (int i = 0; i < loops; i++)
        {
            if (i >= xValue.Length)
            {
                minY++;
            }

            if (i < yValue.Length)
            {
                maxY++;
            }

            var x = i - minY;
            for (int y = minY; y < maxY; y++)
            {
                //int x = (i - y);
                result ^= xValue[x] ^ yValue[y];
                Console.Write($"{x},{y} : ");
                --x;
            }
            Console.WriteLine();
        }

        return result;
    }

    private static int Loop(string xValue, string yValue)
    {
        var result = 0;
        var minX = 0;

        var minY = 0;
        var maxY = 0;

        while (true)
        {
            var x = minX;
            //Console.WriteLine(maxY - minY);
            for (var y = minY; y <= maxY; y++)
            {
                result ^= xValue[x] ^ yValue[y];
                Console.Write($"{x},{y} : ");
                --x;
            }

            Console.WriteLine();

            if (maxY < yValue.Length - 1)
            {
                ++maxY;
            }

            if (minX < xValue.Length - 1)
            {
                minX++;
            }
            else if (minY < yValue.Length - 1)
            {
                ++minY;
            }
            else
            {
                break;
            }
        }

        return result;
    }
}