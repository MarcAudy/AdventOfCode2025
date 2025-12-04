using System.Text.RegularExpressions;

class day3
{
    public static void run()
    {
        var exampleResult = evaluate("day3_example");
        var result = evaluate("day3_input");
        Console.WriteLine($"Day3 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day3 Part1: {result.part1}");
        Console.WriteLine($"Day3 Part2 Example: {exampleResult.part2}");
        Console.WriteLine($"Day3 Part2: {result.part2}");
    }

    private static (ulong part1, ulong part2) evaluate(string fileName)
    {
        ulong part1_totalJoltage = 0;
        ulong part2_totalJoltage = 0;
        FileProcessor.ProcessLines(fileName, line =>
        {
            ulong FindLargestJoltage(List<ulong> batteries, int digits)
            {
                if (digits == 0) return 0;

                int digitIndex = 0;
                for (int index = 1; index < batteries.Count() - digits + 1; index++)
                {
                    if (batteries[index] > batteries[digitIndex])
                    {
                        digitIndex = index;
                    }
                }                
                return batteries[digitIndex] * (ulong)Math.Pow(10, digits-1) + FindLargestJoltage(batteries[(digitIndex+1)..], digits-1);
            }

            List<ulong> batteries = line.Select(ch => (ulong)char.GetNumericValue(ch)).ToList();

            part1_totalJoltage += FindLargestJoltage(batteries, 2);
            part2_totalJoltage += FindLargestJoltage(batteries, 12);
        });
        return (part1_totalJoltage, part2_totalJoltage);
    }
}