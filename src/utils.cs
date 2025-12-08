using System.Runtime.CompilerServices;

class FileProcessor
{
    public static string[] GetLines(string fileName)
    {
        return File.ReadAllLines($"{ProjectInfo.ProjectDir}/../input/{fileName}.txt");
    }

    public static void ProcessLines(string fileName, Action<string> lineAction)
    {
        foreach (string line in GetLines(fileName))
        {
            lineAction(line);
        }
    }

    public static void ParallelProcessLines(string fileName, Action<string> lineAction)
    {
        string[] lines = GetLines(fileName);
        Parallel.For(0, lines.Length, index => lineAction(lines[index]));
    }

    public static void ProcessSegments(string fileName, char separator, Action<string> segmentAction)
    {
        foreach (string segment in File.ReadAllText($"{ProjectInfo.ProjectDir}/../input/{fileName}.txt").Split(separator))
        {
            segmentAction(segment);
        }
    }

    public static void ParallelProcessSegments(string fileName, char separator, Action<string> segmentAction)
    {
        string[] segments = File.ReadAllText($"{ProjectInfo.ProjectDir}/../input/{fileName}.txt").Split(separator);
        Parallel.For(0, segments.Length, index => segmentAction(segments[index]));
    }
}

class Utils
{
    public static void InsertSorted<T>(List<T> list, T item, IComparer<T> comparer)
    {
        int index = list.BinarySearch(item, comparer);
        if (index < 0) index = ~index;
        list.Insert(index, item);
    }

    public static void InsertSorted<T>(List<T> list, T item)
        where T : IComparable<T>
    {
        InsertSorted(list, item, Comparer<T>.Default);
    }

    public static List<T> MergeKSorted<T>(IEnumerable<List<T>> lists, IComparer<T> comparer)
    {
        var result = new List<T>();

        // Min-heap of (value, listIndex, itemIndex)
        var pq = new PriorityQueue<(T value, int listIndex, int itemIndex), T>(comparer);

        var listArray = lists.ToList();

        // Initialize heap
        for (int i = 0; i < listArray.Count; i++)
        {
            if (listArray[i].Count > 0)
            {
                pq.Enqueue((listArray[i][0], i, 0), listArray[i][0]);
            }
        }

        while (pq.Count > 0)
        {
            var (value, li, idx) = pq.Dequeue();
            result.Add(value);

            int nextIdx = idx + 1;
            if (nextIdx < listArray[li].Count)
            {
                var nextVal = listArray[li][nextIdx];
                pq.Enqueue((nextVal, li, nextIdx), nextVal);
            }
        }

        return result;
    }

    public static List<T> MergeKSorted<T>(IEnumerable<List<T>> lists)
    {
        return MergeKSorted(lists, Comparer<T>.Default);
    }
}