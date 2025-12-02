class FileProcessor
{
    public static void Process(string fileName, Action<string> lineAction)
    {
        foreach (string line in File.ReadAllLines($"../../../../input/{fileName}.txt"))
        {
            lineAction(line);
        }
    }
}