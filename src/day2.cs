using System.Text.RegularExpressions;

class day2
{
    public static void run()
    {
        Console.WriteLine($"Day2 Part1 Example: {part1("day2_example")}");
        Console.WriteLine($"Day2 Part1: {part1("day2_input")}");
    }

    private static ulong part1(string fileName)
    {
        ulong invalidIDSum = 0;
        FileProcessor.ProcessSegments(fileName, ',', segment =>
        {
            string[] segments = segment.Split('-');
            ulong rangeStart = ulong.Parse(segments[0]);
            ulong rangeEnd = ulong.Parse(segments[1]);
            Regex regex = new(@"^(.+)\1$");
            for (ulong id = rangeStart; id <= rangeEnd; id++)
            {
                if (regex.Match($"{id}").Success)
                {
                    invalidIDSum += id;
                }
            }
        });
        return invalidIDSum;
    }
}