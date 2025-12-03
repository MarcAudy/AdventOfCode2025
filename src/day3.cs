using System.Text.RegularExpressions;

class day3
{
    public static void run()
    {
        var exampleResult = evaluate("day3_example");
        var result = evaluate("day3_input");
        Console.WriteLine($"Day3 Part1 Example: {exampleResult.Item1}");
        Console.WriteLine($"Day3 Part1: {result.Item1}");
        //Console.WriteLine($"Day3 Part2 Example: {exampleResult.Item2}");
        //Console.WriteLine($"Day3 Part2: {result.Item2}");
    }

    private static (int, int) evaluate(string fileName)
    {
        int totalJoltage = 0;
        FileProcessor.ProcessLines(fileName, line =>
        {
            List<int> batteries = line.Select(ch => (int)char.GetNumericValue(ch)).ToList();
            int firstDigitIndex = 0;
            int secondDigitIndex = 1;
            for (int index = 1; index < batteries.Count(); index++)
            {
                if (index < batteries.Count() - 1 && batteries[index] > batteries[firstDigitIndex])
                {
                    firstDigitIndex = index;
                    secondDigitIndex = index + 1;
                }
                else if (batteries[index] > batteries[secondDigitIndex])
                {
                    secondDigitIndex = index;
                }
            }
            totalJoltage += batteries[firstDigitIndex] * 10 + batteries[secondDigitIndex];
        });
        return (totalJoltage, 0);
    }
}