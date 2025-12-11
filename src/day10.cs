using Google.OrTools.LinearSolver;
using System.Diagnostics;

 class Day10
{
    public static void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();        
        var exampleResult = Evaluate("day10_example");
        var result = Evaluate("day10_input");
        sw.Stop();

        Console.WriteLine($"Day10 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day10 Part1: {result.part1}");
        Console.WriteLine($"Day10 Part2 Example: {exampleResult.part2}");
        Console.WriteLine($"Day10 Part2: {result.part2}");
        Console.WriteLine($"Solved Day10 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (uint part1, uint part2) Evaluate(string fileName)
    {
        uint part1_result = 0;
        uint part2_result = 0;

        FileProcessor.ProcessLines(fileName, line =>
        {
            string[] components = line.Split(' ');
            uint lightTarget = components[0][1..^1].Select((ch, index) => (ch, index)).Aggregate(0U, (mask, lightInfo) => mask |= (lightInfo.ch == '.' ? mask : 1U << lightInfo.index));
            uint[] joltageTarget = [.. components.Last()[1..^1].Split(',').Select(s => uint.Parse(s))];
            uint[][] buttons = [.. components[1..^1].Select(button => button[1..^1].Split(',').Select(s => (uint)char.GetNumericValue(s[0])).ToArray())];
            uint[] buttonMasks = [.. buttons.Select(button => button.Aggregate(0U, (mask, lightID) => mask |= 1U << (int)lightID))];

            // Part 1
            {
                HashSet<uint> seenPatterns = [];
                Queue<(uint pattern, uint buttonMask, uint count)> buttonsToPress = new (buttonMasks.Select(buttonMask => (0U, buttonMask, 0U)));

                while (buttonsToPress.Count > 0)
                {
                    var buttonToPress = buttonsToPress.Dequeue();
                    uint newPattern = buttonToPress.pattern ^ buttonToPress.buttonMask;
                    uint nextCount = buttonToPress.count + 1;
                    if (newPattern == lightTarget)
                    {
                        part1_result += nextCount;
                        break;
                    }
                    if (!seenPatterns.Contains(newPattern))
                    {
                        seenPatterns.Add(newPattern);
                        foreach (uint buttonMask in buttonMasks) 
                        {
                            buttonsToPress.Enqueue((newPattern, buttonMask, nextCount));
                        }
                    }
                }
            }

            // Part 2
            {
                Solver solver = Solver.CreateSolver("CBC");
                Objective objective = solver.Objective();
                objective.SetMinimization();
                Variable[] variables = [.. buttons.Select((_, index) => solver.MakeIntVar(0.0, double.PositiveInfinity, $"{index}"))];
                Constraint[] constraints = [.. joltageTarget.Select(jt => solver.MakeConstraint(jt, jt))];

                for (int buttonIndex = 0; buttonIndex < buttons.Length; buttonIndex++)
                {
                    objective.SetCoefficient(variables[buttonIndex], 1);
                    foreach (uint target in buttons[buttonIndex])
                    {
                        constraints[target].SetCoefficient(variables[buttonIndex], 1);
                    }
                }
                solver.Solve();
                part2_result += (uint)variables.Aggregate(0.0, (sum, v) => sum + v.SolutionValue());
            }
        });

        return (part1_result, part2_result);
    }
}