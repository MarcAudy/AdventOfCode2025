using System.Diagnostics;
using System.Text.RegularExpressions;

partial class Day6
{
    public static void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();        
        var exampleResult = Evaluate("day6_example");
        var result = Evaluate("day6_input");
        sw.Stop();

        Console.WriteLine($"Day6 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day6 Part1: {result.part1}");
        //Console.WriteLine($"Day6 Part2 Example: {exampleResult.part2}");
        //Console.WriteLine($"Day6 Part2: {result.part2}");
        Console.WriteLine($"Solved Day6 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (ulong part1, ulong part2) Evaluate(string fileName)
    {
        string[] lines = FileProcessor.GetLines(fileName);
        ulong[][] numbers = lines[..^1].Select(line => WhiteSpaceSplitRegex().Split(line.Trim()).Select(s => ulong.Parse(s)).ToArray()).ToArray();                                   
        char[] operators = WhiteSpaceSplitRegex().Split(lines.Last().Trim()).Select(s => s[0]).ToArray();

        ulong grandTotal = 0;
        for (int n = 0; n < operators.Length; n++)
        {
            if (operators[n] == '+')
            {
                foreach (ulong[] nums in numbers) 
                {
                    grandTotal += nums[n];
                }
            }
            else
            {
                ulong product = 1;
                foreach (ulong[] nums in numbers) 
                {
                    product *= nums[n];
                }
                grandTotal += product;
            }
        }

        return (grandTotal, 0);
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhiteSpaceSplitRegex();
}