namespace WordleSolver;

public class WordTable : IWordTable
{
    private readonly string[] _words;

    public WordTable()
    {
        _words = File.ReadAllLines("words.txt")
            .Select(w => w.Trim())
            .ToArray();
    }

    public string[] GetWords()
    {
        return _words;
    }
}