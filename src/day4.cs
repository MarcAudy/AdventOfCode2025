using System.Text.RegularExpressions;

class day4
{
    public static void run()
    {
        var exampleResult = evaluate("day4_example");
        var result = evaluate("day4_input");
        Console.WriteLine($"Day4 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day4 Part1: {result.part1}");
        Console.WriteLine($"Day4 Part2 Example: {exampleResult.part2}");
        Console.WriteLine($"Day4 Part2: {result.part2}");
    }

    private static (int part1, int part2) evaluate(string fileName)
    {
        char[][] grid = FileProcessor.GetLines(fileName).Select(line => line.ToCharArray()).ToArray();

        int part1_reachable = 0;
        int part2_reachable = 0;

        while (true)
        {
            HashSet<(int, int)> reachable = new();
            for (int y = 0; y < grid.Count(); y++)
            {
                for (int x = 0; x < grid[y].Count(); x++)
                {
                    if (grid[y][x] == '@')
                    {
                        int neighbors = -1; // to avoid a branch we'll end up counting ourselves, so start at -1
                        (int, int) xOffsetRange = (x > 0 ? -1 : 0, x < grid[y].Count() - 1 ? 1 : 0);
                        (int, int) yOffsetRange = (y > 0 ? -1 : 0, y < grid.Count() - 1 ? 1 : 0);
                        for (int xOffset = xOffsetRange.Item1; xOffset <= xOffsetRange.Item2; xOffset++)
                        {
                            for (int yOffset = yOffsetRange.Item1; yOffset <= yOffsetRange.Item2; yOffset++)
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
            if (reachable.Count() == 0)
            {
                break;
            }
            if (part1_reachable == 0)
            {
                part1_reachable = reachable.Count();
            }
            part2_reachable += reachable.Count();

            foreach ((int x, int y) p in reachable)
            {
                grid[p.y][p.x] = '.';
            }
        }
        return (part1_reachable, part2_reachable);
    }
}