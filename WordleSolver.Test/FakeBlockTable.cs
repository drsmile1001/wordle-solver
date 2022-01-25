namespace WordleSolver.Test;

internal class FakeBlockTable : IBlockWordTable
{
    private readonly string[] _blockWords;

    public FakeBlockTable(string[] blockWords)
    {
        _blockWords = blockWords;
    }

    public string[] GetBlockWords() => _blockWords;
}
