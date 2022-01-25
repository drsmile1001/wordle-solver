namespace WordleSolver;

public class BlockWordTable : IBlockWordTable
{
    private const string BlockWordFileName = "block-words.txt";

    public void AddWord(string word)
    {
        try
        {
            File.AppendAllLines(BlockWordFileName, new[] { word });
        }
        catch (FileNotFoundException)
        {

            File.WriteAllLines(BlockWordFileName, new[] { word });
        }
    }

    public string[] GetBlockWords()
    {
        try
        {
            return File.ReadAllLines(BlockWordFileName);
        }
        catch (FileNotFoundException)
        {
            return Array.Empty<string>();
        }
    }
}