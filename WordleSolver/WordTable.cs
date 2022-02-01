namespace WordleSolver;

public class WordTable : IWordTable
{
    private readonly Lazy<string[]> _words = new(() =>
        File.ReadAllLines("words.txt")
            .Select(w => w.Trim())
            .ToArray()
    );

    public string[] GetWords()
    {
        return _words.Value;
    }
}