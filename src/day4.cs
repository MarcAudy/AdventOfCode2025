using System.Diagnostics;

class Day4
{
    public static void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();        
        var exampleResult = Evaluate("day4_example");
        var result = Evaluate("day4_input");
        sw.Stop();

        Console.WriteLine($"Day4 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day4 Part1: {result.part1}");
        Console.WriteLine($"Day4 Part2 Example: {exampleResult.part2}");
        Console.WriteLine($"Day4 Part2: {result.part2}");
        Console.WriteLine($"Solved Day 4 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (int part1, int part2) Evaluate(string fileName)
    {
        char[][] grid = [.. FileProcessor.GetLines(fileName).Select(line => line.ToCharArray())];

        int part1_reachable = 0;
        int part2_reachable = 0;

        while (true)
        {
            List<(int, int)> reachable = [];
            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    if (grid[y][x] == '@')
                    {
                        int neighbors = -1; // to avoid a branch we'll end up counting ourselves, so start at -1
                        (int start, int end) xOffsetRange = (x > 0 ? -1 : 0, x < grid[y].Length - 1 ? 1 : 0);
                        (int start, int end) yOffsetRange = (y > 0 ? -1 : 0, y < grid.Length - 1 ? 1 : 0);
                        for (int xOffset = xOffsetRange.start; xOffset <= xOffsetRange.end; xOffset++)
                        {
                            for (int yOffset = yOffsetRange.start; yOffset <= yOffsetRange.end; yOffset++)
                            {
                                if (grid[y+yOffset][x+xOffset] == '@')
                                {
                                    neighbors++;
                                }
                            }
                        }
                        if (neighbors < 4)
                        {
                            reachable.Add((x,y));
                        }
                    }
                }
            }
            if (reachable.Count == 0)
            {
                break;
            }
            if (part1_reachable == 0)
            {
                part1_reachable = reachable.Count;
            }
            part2_reachable += reachable.Count;

            foreach ((int x, int y) p in reachable)
            {
                grid[p.y][p.x] = '.';
            }
        }
        return (part1_reachable, part2_reachable);
    }
}