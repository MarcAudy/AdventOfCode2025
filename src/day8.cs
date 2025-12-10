using System.Collections.Concurrent;
using System.Diagnostics;

using JunctionBox = ((ulong x, ulong y, ulong z) location, int circuit_id);
using JunctionBoxDistance = (int box1_id, int box2_id, ulong distanceSq);

class JunctionBoxDistanceComparer : IComparer<JunctionBoxDistance>
{
    public int Compare(JunctionBoxDistance a, JunctionBoxDistance b)
    {
        return a.distanceSq.CompareTo(b.distanceSq);
    }
}

class Day8
{
    public static void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();        
        var exampleResult = Evaluate("day8_example", 10);
        var result = Evaluate("day8_input", 1000);
        sw.Stop();

        Console.WriteLine($"Day8 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day8 Part1: {result.part1}");
        Console.WriteLine($"Day8 Part2 Example: {exampleResult.part2}");
        Console.WriteLine($"Day8 Part2: {result.part2}");
        Console.WriteLine($"Solved Day8 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (int part1, ulong part2) Evaluate(string fileName, int numConnections)
    {
        JunctionBox[] boxes = [.. FileProcessor.GetLines(fileName).Select(line =>
        {
            ulong[] nums = [.. line.Split(',').Select(s => ulong.Parse(s))];
            return ((nums[0], nums[1], nums[2]), -1);
        })];

        ConcurrentBag<List<JunctionBoxDistance>> allLists = [];
        JunctionBoxDistanceComparer jbdComparer = new();
        Parallel.For(0, boxes.Length,
            () => new List<JunctionBoxDistance>(),
            (box1_id, _, localSortedList) => {
                for (int box2_id = box1_id + 1; box2_id < boxes.Length; box2_id++)
                {
                    ulong dx = boxes[box1_id].location.x - boxes[box2_id].location.x;
                    ulong dy = boxes[box1_id].location.y - boxes[box2_id].location.y;
                    ulong dz = boxes[box1_id].location.z - boxes[box2_id].location.z;
                    ulong distanceSq = dx * dx + dy * dy + dz * dz;
                    Utils.InsertSorted(localSortedList, (box1_id, box2_id, distanceSq), jbdComparer);
                }
                return localSortedList;
            },
            localSortedList => { 
                allLists.Add(localSortedList);
            });

        List<List<int>> circuits = [];
        List<JunctionBoxDistance> distancesSq = Utils.MergeKSorted(allLists, jbdComparer);

        int part1_result = -1;
        ulong part2_result = 0;

        for (int connection = 0; true; connection++)
        {
            JunctionBoxDistance jbd = distancesSq[connection];
            int new_circuit_id = -1;
            if (boxes[jbd.box1_id].circuit_id != -1)
            {
                if (boxes[jbd.box2_id].circuit_id != -1)
                {
                    if (boxes[jbd.box1_id].circuit_id != boxes[jbd.box2_id].circuit_id)
                    {
                        new_circuit_id = boxes[jbd.box1_id].circuit_id;
                        int old_circuit_id = boxes[jbd.box2_id].circuit_id;
                        foreach (int id in circuits[old_circuit_id])
                        {
                            boxes[id].circuit_id = new_circuit_id;
                        }
                        circuits[new_circuit_id].AddRange(circuits[old_circuit_id]);
                        circuits[old_circuit_id].Clear();
                    }
                }
                else
                {
                    new_circuit_id = boxes[jbd.box1_id].circuit_id;
                    boxes[jbd.box2_id].circuit_id = new_circuit_id;
                    circuits[new_circuit_id].Add(jbd.box2_id);
                }
            }
            else if (boxes[jbd.box2_id].circuit_id != -1)
            {
                new_circuit_id = boxes[jbd.box2_id].circuit_id;
                boxes[jbd.box1_id].circuit_id = new_circuit_id;
                circuits[new_circuit_id].Add(jbd.box1_id);
            }
            else
            {
                new_circuit_id = circuits.Count;
                boxes[jbd.box1_id].circuit_id = new_circuit_id;
                boxes[jbd.box2_id].circuit_id = new_circuit_id;
                circuits.Add([jbd.box1_id, jbd.box2_id]);
            }
            if (connection + 1 == numConnections)
            {
                int[] largestCircuits = [0, 0, 0];
                foreach (List<int> circuit in circuits)
                {
                    if (circuit.Count > largestCircuits[2])
                    {
                        largestCircuits[2] = circuit.Count;
                        if (largestCircuits[2] > largestCircuits[1])
                        {
                            (largestCircuits[1], largestCircuits[2]) = (largestCircuits[2], largestCircuits[1]);
                            if (largestCircuits[1] > largestCircuits[0])
                            {
                                (largestCircuits[0], largestCircuits[1]) = (largestCircuits[1], largestCircuits[0]);
                            }
                        }
                    }
                }
                part1_result = largestCircuits[0] * largestCircuits[1] * largestCircuits[2];
            }
            if (new_circuit_id >= 0 && circuits[new_circuit_id].Count == boxes.Length)
            {
                part2_result = boxes[jbd.box1_id].location.x * boxes[jbd.box2_id].location.x;
                break;
            }
        }

        return (part1_result, part2_result);
    }
}