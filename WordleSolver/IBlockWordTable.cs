namespace WordleSolver;

public interface IBlockWordTable
{
    string[] GetBlockWords();

    void AddWord(string word);
}