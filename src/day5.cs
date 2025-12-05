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
        Console.WriteLine($"Day5 Part2 Example: {exampleResult.part2}");
        Console.WriteLine($"Day5 Part2: {result.part2}");
        Console.WriteLine($"Solved Day5 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (int part1, ulong part2) Evaluate(string fileName)
    {
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
            ulong rangeStart = ulong.Parse(rangeEnds[0]);
            ulong rangeEnd = ulong.Parse(rangeEnds[1]);
            bool addRange = true;

            // Remove overlaps by checking existing ranges
            for (int n = freshRanges.Count - 1; n >= 0; n--)
            {
                // There are 5 scenarios to consider
                (ulong start, ulong end) = freshRanges[n];
                
                // The new range is entirely inside the existing range
                if (rangeStart >= start && rangeEnd <= end)
                {
                    addRange = false;
                    break;
                }
                // The existing range is entirely inside the new range
                else if (rangeStart <= start && rangeEnd >= end)
                {
                    freshRanges.RemoveAt(n);
                }
                // The new range starts earlier and overlaps
                else if (rangeStart < start && rangeEnd >= start)
                {
                    rangeEnd = start - 1;
                }
                // The new range extends further but overlaps at the start
                else if (rangeEnd > end && rangeStart <= end)
                {
                    rangeStart = end + 1;
                }
                // Otherwise there is no overlap and nothing to do
            }

            if (addRange)
            {
                freshRanges.Add((rangeStart, rangeEnd));
            }
        }

        int part1_fresh = 0;
        for (; index < lines.Length; index++)
        {
            ulong ingredientID = ulong.Parse(lines[index]);
            if (freshRanges.Any(((ulong start, ulong end) range) => ingredientID >= range.start && ingredientID <= range.end))
            {
                part1_fresh++;
            }
        }

        ulong part2_fresh = 0;
        foreach ((ulong start, ulong end) in freshRanges)
        {
            part2_fresh += end - start + 1;
        }

        return (part1_fresh, part2_fresh);
    }
}