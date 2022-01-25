namespace WordleSolver;

public class WordTable : IWordTable
{
    public string[] GetWords()
    {
        return File.ReadAllLines("words.txt")
            .Select(w => w.Trim())
            .ToArray();
    }
}