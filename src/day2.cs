using System.Diagnostics;
using System.Text.RegularExpressions;

partial class Day2
{
    public static void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();        
        var exampleResult = Evaluate("day2_example");
        var result = Evaluate("day2_input");
        sw.Stop();

        Console.WriteLine($"Day2 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day2 Part1: {result.part1}");
        Console.WriteLine($"Day2 Part2 Example: {exampleResult.part2}");
        Console.WriteLine($"Day2 Part2: {result.part2}");
        Console.WriteLine($"Solved Day 2 in {sw.Elapsed.TotalSeconds} seconds");

    }

    private static (long part1, long part2) Evaluate(string fileName)
    {
        long part1_invalidIDSum = 0;
        long part2_invalidIDSum = 0;
        FileProcessor.ParallelProcessSegments(fileName, ',', segment =>
        {
            string[] segments = segment.Split('-');
            long rangeStart = long.Parse(segments[0]);
            long rangeEnd = long.Parse(segments[1]);
            Parallel.For(rangeStart, rangeEnd+1, id =>
            {
                if (part1_regex().Match($"{id}").Success)
                {
                    part1_invalidIDSum += id;
                    part2_invalidIDSum += id;
                }
                else if (part2_regex().Match($"{id}").Success)
                {
                    part2_invalidIDSum += id;
                }
            });
        });
        return (part1_invalidIDSum, part2_invalidIDSum);
    }

    [GeneratedRegex(@"^(.+)\1$")]
    private static partial Regex part1_regex();
    [GeneratedRegex(@"^(.+)\1+$")]
    private static partial Regex part2_regex();
}