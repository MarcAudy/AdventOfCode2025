using System.Collections.Concurrent;
using System.Diagnostics;
using System.Numerics;

using JunctionBox = (System.Numerics.Vector3 location, int circuit_id);
using JunctionBoxDistance = (int box1_id, int box2_id, float distanceSq);

class JunctionBoxDistanceComparer : IComparer<JunctionBoxDistance>
{
    public int Compare(JunctionBoxDistance a, JunctionBoxDistance b)
    {
        return a.distanceSq.CompareTo(b.distanceSq);
    }
}

partial class Day8
{
    
    public static void Run()
    {
        Stopwatch sw = Stopwatch.StartNew();        
        //var exampleResult = Evaluate("day8_example", 10);
        var result = Evaluate("day8_input", 1000);
        sw.Stop();

        //Console.WriteLine($"Day8 Part1 Example: {exampleResult.part1}");
        Console.WriteLine($"Day8 Part1: {result.part1}");
        //Console.WriteLine($"Day8 Part2 Example: {exampleResult.part2}");
        //Console.WriteLine($"Day8 Part2: {result.part2}");
        Console.WriteLine($"Solved Day8 in {sw.Elapsed.TotalSeconds} seconds");
    }

    private static (int part1, int part2) Evaluate(string fileName, int numConnections)
    {
        JunctionBox[] boxes = [.. FileProcessor.GetLines(fileName).Select(line =>
        {
            float[] nums = [.. line.Split(',').Select(s => float.Parse(s))];
            return (new Vector3(nums[0], nums[1], nums[2]), -1);
        })];

        ConcurrentBag<List<JunctionBoxDistance>> allLists = [];
        JunctionBoxDistanceComparer jbdComparer = new();
        Parallel.For<List<JunctionBoxDistance>>(0, boxes.Length,
            () => new List<JunctionBoxDistance>(),
            (box1_id, _, localSortedList) => {
                for (int box2_id = box1_id + 1; box2_id < boxes.Length; box2_id++)
                {
                    float distanceSq = Vector3.DistanceSquared(boxes[box1_id].location, boxes[box2_id].location);
                    Utils.InsertSorted(localSortedList, (box1_id, box2_id, distanceSq), jbdComparer);
                }
                return localSortedList;
            },
            localSortedList => { 
                allLists.Add(localSortedList);
        });

        List<List<int>> circuits = [];
        List<JunctionBoxDistance> distancesSq = Utils.MergeKSorted(allLists, jbdComparer);

        for (int connection = 0; connection < numConnections; connection++)
        {
            JunctionBoxDistance jbd = distancesSq[connection];
            if (boxes[jbd.box1_id].circuit_id != -1)
            {
                if (boxes[jbd.box2_id].circuit_id != -1)
                {
                    if (boxes[jbd.box1_id].circuit_id != boxes[jbd.box2_id].circuit_id)
                    {
                        int new_circuit_id = boxes[jbd.box1_id].circuit_id;
                        int old_circuit_id = boxes[jbd.box2_id].circuit_id;
                        foreach (int id in circuits[old_circuit_id])
                        {
                            boxes[id].circuit_id = new_circuit_id;
                            circuits[new_circuit_id].Add(id);
                        }
                        circuits[old_circuit_id].Clear();
                    }
                }
                else
                {
                    boxes[jbd.box2_id].circuit_id = boxes[jbd.box1_id].circuit_id;
                    circuits[boxes[jbd.box1_id].circuit_id].Add(jbd.box2_id);
                }
            }
            else if (boxes[jbd.box2_id].circuit_id != -1)
            {
                boxes[jbd.box1_id].circuit_id = boxes[jbd.box2_id].circuit_id;
                circuits[boxes[jbd.box2_id].circuit_id].Add(jbd.box1_id);
            }
            else
            {
                int circuit_id = circuits.Count;
                boxes[jbd.box1_id].circuit_id = circuit_id;
                boxes[jbd.box2_id].circuit_id = circuit_id;
                circuits.Add([jbd.box1_id, jbd.box2_id]);
            }
        }

        circuits.Sort((a, b) => b.Count.CompareTo(a.Count));
        
        int part1_result = circuits[0].Count * circuits[1].Count * circuits[2].Count;

        return (part1_result, 0);
    }
}