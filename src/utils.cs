class FileProcessor
{
    public static void ProcessLines(string fileName, Action<string> lineAction)
    {
        foreach (string line in File.ReadAllLines($"../../../../input/{fileName}.txt"))
        {
            lineAction(line);
        }
    }

    public static string[] GetLines(string fileName)
    {
        return File.ReadAllLines($"../../../../input/{fileName}.txt");
    }

    public static void ProcessSegments(string fileName, char separator, Action<string> segmentAction)
    {
        foreach (string segment in File.ReadAllText($"../../../../input/{fileName}.txt").Split(separator))
        {
            segmentAction(segment);
        }
    }
}