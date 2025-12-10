using System.Diagnostics;

class Day7
{
    public static void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();        
        var exampleResult = Evaluate("day7_example");
        var result = Evaluate("day7_input");
        sw.Stop();

        Console.WriteLine($"Day7 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day7 Part1: {result.part1}");
        Console.WriteLine($"Day7 Part2 Example: {exampleResult.part2}");
        Console.WriteLine($"Day7 Part2: {result.part2}");
        Console.WriteLine($"Solved Day7 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (int part1, ulong part2) Evaluate(string fileName)
    {
        string[] lines = FileProcessor.GetLines(fileName);
        (char ch, ulong ways)[][] grid = lines.Select(line => line.Select(ch => (ch, ch == 'S' ? 1UL : 0)).ToArray()).ToArray();

        int part1_splits = 0;
        for (int y = 1; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y-1][x].ch == 'S' || grid[y-1][x].ch == '|')
                {
                    if (grid[y][x].ch == '^')
                    {
                        part1_splits++;
                        if (x > 0)
                        {
                            grid[y][x-1].ch = '|';
                            grid[y][x-1].ways += grid[y-1][x].ways;
                        }
                        if (x < grid[y].Length - 1)
                        {
                            grid[y][x+1].ch = '|';    
                            grid[y][x+1].ways += grid[y-1][x].ways;
                        }
                    }
                    else
                    {
                        grid[y][x].ch = '|';
                        grid[y][x].ways += grid[y-1][x].ways;
                    }
                }
            }
        }

        ulong part2_timelines = grid.Last().Aggregate(0UL, (sum, x) => sum + x.ways);

        return (part1_splits, part2_timelines);
    }
}