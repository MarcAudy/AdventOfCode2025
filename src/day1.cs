class day1
{
    public static void run()
    {
        Console.WriteLine($"Day1 Part1 Example: {part1("day1_example")}");
        Console.WriteLine($"Day1 Part1: {part1("day1_input")}");
        Console.WriteLine($"Day1 Part2 Example: {part2("day1_example")}");
        Console.WriteLine($"Day1 Part2: {part2("day1_input")}");
    }

    private static int part1(string fileName)
    {
        int dial = 50;
        int zeroes = 0;
        FileProcessor.ProcessLines(fileName, line =>
        {
            int movement = int.Parse(line[1..]);
            if (line[0] == 'L')
            {
                dial -= movement;
                while (dial < 0)
                {
                    dial += 100;
                }
            }
            else
            {
                dial += movement;
                dial %= 100;
            }
            if (dial == 0)
            {
                zeroes++;
            }
        });
        return zeroes;
    }

    private static int part2(string fileName)
    {
        int dial = 50;
        int zeroes = 0;
        FileProcessor.ProcessLines(fileName, line =>
        {
            int movement = int.Parse(line[1..]);
            zeroes += movement / 100;
            movement %= 100;
            if (line[0] == 'L')
            {
                if (dial == 0)
                {
                    zeroes--;
                }
                dial -= movement;
                if (dial == 0)
                {
                    zeroes++;
                }
                if (dial < 0)
                {
                    dial += 100;
                    zeroes++;
                }
            }
            else
            {
                dial += movement;
                if (dial >= 100)
                {
                    dial -= 100;
                    zeroes++;
                }
            }
        });
        return zeroes;
    }    
}