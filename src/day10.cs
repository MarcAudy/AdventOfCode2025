using System.Collections.Concurrent;
using System.Diagnostics;

partial class Day10
{
    
    public static void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();        
        var exampleResult = Evaluate("day10_example");
        var result = Evaluate("day10_input");
        sw.Stop();

        Console.WriteLine($"Day10 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day10 Part1: {result.part1}");
        //Console.WriteLine($"Day10 Part2 Example: {exampleResult.part2}");
        //Console.WriteLine($"Day10 Part2: {result.part2}");
        Console.WriteLine($"Solved Day10 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (uint part1, ulong part2) Evaluate(string fileName)
    {
        uint part1_result = 0;

        FileProcessor.ProcessLines(fileName, line =>
        {
            string[] components = line.Split(' ');
            uint lightTarget = components[0][1..^1].Select((ch, index) => (ch, index)).Aggregate(0U, (mask, lightInfo) => mask |= (lightInfo.ch == '.' ? mask : 1U << lightInfo.index));
            uint[] buttons = [.. components[1..^1].Select(button => button[1..^1].Split(',').Aggregate(0U, (mask, lightID) => mask |= 1U << (int)char.GetNumericValue(lightID[0])))];

            HashSet<uint> seenPatterns = [];
            Queue<(uint pattern, uint button, uint count)> buttonsToPress = new (buttons.Select(button => (0U, button, 0U)));

            while (buttonsToPress.Count > 0)
            {
                var buttonToPress = buttonsToPress.Dequeue();
                uint newPattern = buttonToPress.pattern ^ buttonToPress.button;
                uint nextCount = buttonToPress.count + 1;
                if (newPattern == lightTarget)
                {
                    part1_result += nextCount;
                    break;
                }
                if (!seenPatterns.Contains(newPattern))
                {
                    seenPatterns.Add(newPattern);
                    foreach (uint button in buttons) 
                    {
                        buttonsToPress.Enqueue((newPattern, button, nextCount));
                    }
                }
            }
        });

        return (part1_result, 0);
    }
}