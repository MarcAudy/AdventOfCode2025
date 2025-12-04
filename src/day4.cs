using System.Text.RegularExpressions;

class day4
{
    public static void run()
    {
        var exampleResult = evaluate("day4_example");
        var result = evaluate("day4_input");
        Console.WriteLine($"Day4 Part1 Example: {exampleResult}");
        Console.WriteLine($"Day4 Part1: {result}");
    }

    private static int evaluate(string fileName)
    {
        string[] lines = FileProcessor.GetLines(fileName);

        int reachable = 0;
        for (int y = 0; y < lines.Count(); y++)
        {
            for (int x = 0; x < lines[y].Count(); x++)
            {
                if (lines[y][x] == '@')
                {
                    int neighbors = -1; // to avoid a branch we'll end up counting ourselves, so start at -1
                    (int, int) xOffsetRange = (x > 0 ? -1 : 0, x < lines[y].Count() - 1 ? 1 : 0);
                    (int, int) yOffsetRange = (y > 0 ? -1 : 0, y < lines.Count() - 1 ? 1 : 0);
                    for (int xOffset = xOffsetRange.Item1; xOffset <= xOffsetRange.Item2; xOffset++)
                    {
                        for (int yOffset = yOffsetRange.Item1; yOffset <= yOffsetRange.Item2; yOffset++)
                        {
                            if (lines[y+yOffset][x+xOffset] == '@')
                            {
                                neighbors++;
                            }
                        }
                    }
                    if (neighbors < 4)
                    {
                        reachable++;
                    }
                }
            }
        }        
        return reachable;
    }
}