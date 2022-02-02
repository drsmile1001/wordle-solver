namespace WordleSolver;

public class WordSet
{
    private readonly Dictionary<char, int> _charCount;
    private readonly Dictionary<(char, int), int> _charCountAtPosition;

    public WordSet(string[] words)
    {
        _charCount = words
            .SelectMany(w => w)
            .GroupBy(c => c)
            .ToDictionary(g => g.Key, g => g.Count());
        _charCountAtPosition = words
            .SelectMany(w => w.Select<char,(char,int)>((c,i) => new (c,i)))
            .GroupBy(item => item)
            .ToDictionary(g => g.Key, g=>g.Count());
    }

    public int GetCharCount(char v)
    {
        if(_charCount.TryGetValue(v, out var count))
            return count;
        return 0;
    }

    public int GetCharCountAtPosition(char c, int position)
    {
        if (_charCountAtPosition.TryGetValue(new(c, position), out var count))
            return count;
        return 0;
    }
}
