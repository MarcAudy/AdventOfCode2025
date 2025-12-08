using System.Diagnostics;

partial class Day7
{
    public static void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();        
        var exampleResult = Evaluate("day7_example");
        var result = Evaluate("day7_input");
        sw.Stop();

        Console.WriteLine($"Day7 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day7 Part1: {result.part1}");
        //Console.WriteLine($"Day7 Part2 Example: {exampleResult.part2}");
        //Console.WriteLine($"Day7 Part2: {result.part2}");
        Console.WriteLine($"Solved Day7 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (int part1, int part2) Evaluate(string fileName)
    {
        string[] lines = FileProcessor.GetLines(fileName);
        char[][] grid = lines.Select(line => line.Select(ch => ch).ToArray()).ToArray();

        int part1_splits = 0;
        for (int y = 1; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y-1][x] == 'S' || grid[y-1][x] == '|')
                {
                    if (grid[y][x] == '^')
                    {
                        part1_splits++;
                        if (x > 0)
                        {
                            grid[y][x-1] = '|';
                        }
                        if (x < grid[y].Length - 1)
                        {
                            grid[y][x+1] = '|';    
                        }
                    }
                    else
                    {
                        grid[y][x] = '|';
                    }

                }
            }
        }


        return (part1_splits, 0);
    }
}