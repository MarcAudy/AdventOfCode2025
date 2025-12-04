class Day1
{
    public static void Run()
    {
        var exampleResult = Evaluate("day1_example");
        var result = Evaluate("day1_input");
        Console.WriteLine($"Day1 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day1 Part1: {result.part1}");
        Console.WriteLine($"Day1 Part2 Example: {exampleResult.part2}");
        Console.WriteLine($"Day1 Part2: {result.part2}");
    }

    private static (int part1, int part2) Evaluate(string fileName)
    {
        int dial = 50;
        int part1_zeroes = 0;
        int part2_zeroes = 0;
        FileProcessor.ProcessLines(fileName, line =>
        {
            int movement = int.Parse(line[1..]);
            part2_zeroes += movement / 100;
            movement %= 100;
            if (line[0] == 'L')
            {
                if (dial == 0)
                {
                    part2_zeroes--;
                }
                dial -= movement;
                if (dial == 0)
                {
                    part2_zeroes++;
                }
                if (dial < 0)
                {
                    dial += 100;
                    part2_zeroes++;
                }
            }
            else
            {
                dial += movement;
                if (dial >= 100)
                {
                    dial -= 100;
                    part2_zeroes++;
                }
            }
            if (dial == 0)
            {
                part1_zeroes++;
            }
        });
        return (part1_zeroes, part2_zeroes);
    }    
}