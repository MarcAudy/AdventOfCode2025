using System.Diagnostics;
using System.Text.RegularExpressions;

class Day3
{
    public static void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();        
        var exampleResult = Evaluate("day3_example");
        var result = Evaluate("day3_input");
        sw.Stop();

        Console.WriteLine($"Day3 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day3 Part1: {result.part1}");
        Console.WriteLine($"Day3 Part2 Example: {exampleResult.part2}");
        Console.WriteLine($"Day3 Part2: {result.part2}");
        Console.WriteLine($"Solved Day 3 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (ulong part1, ulong part2) Evaluate(string fileName)
    {
        ulong part1_totalJoltage = 0;
        ulong part2_totalJoltage = 0;
        FileProcessor.ProcessLines(fileName, line =>
        {
            static ulong FindLargestJoltage(ulong[] batteries, int digits)
            {
                if (digits == 0) return 0;

                int digitIndex = 0;
                for (int index = 1; index < batteries.Length - digits + 1; index++)
                {
                    if (batteries[index] > batteries[digitIndex])
                    {
                        digitIndex = index;
                    }
                }                
                return batteries[digitIndex] * (ulong)Math.Pow(10, digits-1) + FindLargestJoltage(batteries[(digitIndex+1)..], digits-1);
            }

            ulong[] batteries = [.. line.Select(ch => (ulong)char.GetNumericValue(ch))];

            part1_totalJoltage += FindLargestJoltage(batteries, 2);
            part2_totalJoltage += FindLargestJoltage(batteries, 12);
        });
        return (part1_totalJoltage, part2_totalJoltage);
    }
}