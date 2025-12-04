class FileProcessor
{
    public static string[] GetLines(string fileName)
    {
        return File.ReadAllLines($"../../../../input/{fileName}.txt");
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
        foreach (string segment in File.ReadAllText($"../../../../input/{fileName}.txt").Split(separator))
        {
            segmentAction(segment);
        }
    }

    public static void ParallelProcessSegments(string fileName, char separator, Action<string> segmentAction)
    {
        string[] segments = File.ReadAllText($"../../../../input/{fileName}.txt").Split(separator);
        Parallel.For(0, segments.Length, index => segmentAction(segments[index]));
    }
}
