namespace WordleSolver.Test;

internal class FakeWordTable : IWordTable
{
    private readonly string[] _words;

    public FakeWordTable(string[] words)
    {
        _words = words;
    }

    public string[] GetWords()
    {
        return _words;
    }
}