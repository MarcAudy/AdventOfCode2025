using System.Diagnostics;

class Day5
{
    public static void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();        
        var exampleResult = Evaluate("day5_example");
        var result = Evaluate("day5_input");
        sw.Stop();

        Console.WriteLine($"Day5 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day5 Part1: {result.part1}");
        //Console.WriteLine($"Day5 Part2 Example: {exampleResult.part2}");
        //Console.WriteLine($"Day5 Part2: {result.part2}");
        Console.WriteLine($"Solved Day5 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (int part1, int part2) Evaluate(string fileName)
    {
        int part1_fresh = 0;

        string[] lines = FileProcessor.GetLines(fileName);

        List<(ulong, ulong)> freshRanges = [];

        int index = 0;
        for (; index < lines.Length; index++)
        {
            if (lines[index].Length == 0)
            {
                index++;
                break;
            }
            string[] rangeEnds = lines[index].Split('-');
            freshRanges.Add((ulong.Parse(rangeEnds[0]),ulong.Parse(rangeEnds[1])));
        }

        for (; index < lines.Length; index++)
        {
            ulong ingredientID = ulong.Parse(lines[index]);
            if (freshRanges.Any(((ulong start, ulong end) range) => ingredientID >= range.start && ingredientID <= range.end))
            {
                part1_fresh++;
            }
        }

        return (part1_fresh, 0);
    }
}