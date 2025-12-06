using System.Diagnostics;

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
        Console.WriteLine($"Day6 Part2 Example: {exampleResult.part2}");
        Console.WriteLine($"Day6 Part2: {result.part2}");
        Console.WriteLine($"Solved Day6 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (ulong part1, ulong part2) Evaluate(string fileName)
    {
        string[] lines = FileProcessor.GetLines(fileName);
        ulong[][] numbers = lines[..^1].Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => ulong.Parse(s)).ToArray()).ToArray();                                   
        char[] operators = lines.Last().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => s[0]).ToArray();

        List<ulong>[] cephalopodNumbers = [.. Enumerable.Range(0, operators.Length).Select(_ => new List<ulong>())];

        int numSet = 0;
        for (int charIndex = 0; charIndex < lines[0].Length; charIndex++)
        {
            ulong cephalopodNumber = 0;
            for (int lineIndex = 0; lineIndex < lines.Length - 1; lineIndex++)
            {
                if (lines[lineIndex][charIndex] != ' ')
                {
                    cephalopodNumber *= 10;
                    cephalopodNumber += (ulong)char.GetNumericValue(lines[lineIndex][charIndex]);
                }
            }
            if (cephalopodNumber > 0)
            {
                cephalopodNumbers[numSet].Add(cephalopodNumber);
            }
            else
            {
                numSet++;
            }
        }

        ulong part1_grandTotal = 0;
        ulong part2_grandTotal = 0;
        for (int opIndex = 0; opIndex < operators.Length; opIndex++)
        {
            if (operators[opIndex] == '+')
            {
                part1_grandTotal += numbers.Aggregate((ulong) 0, (sum, nums) => sum + nums[opIndex]);
                part2_grandTotal += cephalopodNumbers[opIndex].Aggregate((sum, num) => sum + num);
            }
            else
            {
                part1_grandTotal += numbers.Aggregate((ulong)1, (product, nums) => product * nums[opIndex]);
                part2_grandTotal += cephalopodNumbers[opIndex].Aggregate((product, num) => product * num);

            }
        }

        return (part1_grandTotal, part2_grandTotal);
    }
}