using System.Collections.Generic;

namespace WordleSolver.Test;

internal class FakeBlockTable : IBlockWordTable
{
    private readonly List<string> _blockWords;

    public FakeBlockTable(string[] blockWords)
    {
        _blockWords = new List<string>(blockWords);
    }

    public void AddWord(string word)
    {
        _blockWords.Add(word);
    }

    public string[] GetBlockWords() => _blockWords.ToArray();
}
