using System.Collections.Concurrent;
using System.Diagnostics;

class Day9
{
    public static void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();        
        var exampleResult = Evaluate("day9_example");
        var result = Evaluate("day9_input");
        sw.Stop();

        Console.WriteLine($"Day9 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day9 Part1: {result.part1}");
        //Console.WriteLine($"Day9 Part2 Example: {exampleResult.part2}");
        //Console.WriteLine($"Day9 Part2: {result.part2}");
        Console.WriteLine($"Solved Day9 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (ulong part1, ulong part2) Evaluate(string fileName)
    {
        (int x, int y)[] tiles = [.. FileProcessor.GetLines(fileName).Select(line =>
        {
            int[] nums = [.. line.Split(',').Select(s => int.Parse(s))];
            return (nums[0], nums[1]);
        })];

        ConcurrentBag<ulong> allMaxAreas = [];
        Parallel.For(0, tiles.Length,
            () => 0UL,
            (tile1_id, _, localMax) => {
                for (int tile2_id = tile1_id + 1; tile2_id < tiles.Length; tile2_id++)
                {
                    localMax = Math.Max(localMax, (ulong)Math.Abs(tiles[tile1_id].x - tiles[tile2_id].x + 1) * (ulong)Math.Abs(tiles[tile1_id].y - tiles[tile2_id].y + 1));
                }
                return localMax;
            },
            localMax => { 
                allMaxAreas.Add(localMax);
            });

        ulong part1_result = allMaxAreas.Aggregate((max, area) => max = Math.Max(max, area));

        return (part1_result, 0);
    }
}