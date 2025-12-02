class day1
{
    public static void run()
    {
        Console.WriteLine($"Day1 Example: {part1("day1_example")}");
        Console.WriteLine($"Day1: {part1("day1_input")}");
    }

    private static int part1(string fileName)
    {
        int dial = 50;
        int zeroes = 0;
        FileProcessor.Process(fileName, line =>
        {
            if (line[0] == 'L')
            {
                dial += int.Parse(line[1..]);
                dial %= 100;
            }
            else
            {
                dial -= int.Parse(line[1..]);
                while (dial < 0)
                {
                    dial += 100;
                }
            }
            if (dial == 0)
            {
                zeroes++;
            }
        });
        return zeroes;
    }
}