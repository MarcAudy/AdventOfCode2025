using System.Text.RegularExpressions;

partial class Day2
{
    public static void Run()
    {
        var exampleResult = Evaluate("day2_example");
        var result = Evaluate("day2_input");
        Console.WriteLine($"Day2 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day2 Part1: {result.part1}");
        Console.WriteLine($"Day2 Part2 Example: {exampleResult.part2}");
        Console.WriteLine($"Day2 Part2: {result.part2}");
    }

    private static (ulong part1, ulong part2) Evaluate(string fileName)
    {
        ulong part1_invalidIDSum = 0;
        ulong part2_invalidIDSum = 0;
        FileProcessor.ProcessSegments(fileName, ',', segment =>
        {
            string[] segments = segment.Split('-');
            ulong rangeStart = ulong.Parse(segments[0]);
            ulong rangeEnd = ulong.Parse(segments[1]);
            for (ulong id = rangeStart; id <= rangeEnd; id++)
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
            }
        });
        return (part1_invalidIDSum, part2_invalidIDSum);
    }

    [GeneratedRegex(@"^(.+)\1$")]
    private static partial Regex part1_regex();
    [GeneratedRegex(@"^(.+)\1+$")]
    private static partial Regex part2_regex();
}