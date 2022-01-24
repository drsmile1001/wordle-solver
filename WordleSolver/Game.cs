using System.Text.RegularExpressions;

namespace WordleSolver;

public class Game
{
    private readonly List<string> _candidates = new();
    private readonly Regex _validWordPattern = new("^[a-z]{5}$");

    public Game(IWordTable wordTable)
    {
        _candidates = wordTable.GetWords()
            .Where(w => w.Length == 5)
            .Select(w => w.ToLower())
            .Where(w => _validWordPattern.IsMatch(w))
            .ToList();

        _candidates.Sort((a, b) =>
        {
            var dupCompar = a.GroupBy(c => c).Count().CompareTo(b.GroupBy(c => c).Count());
            if (dupCompar != 0) return -dupCompar;
            return a.CompareTo(b);
        });
    }

    public string? NextGuess()
    {
        return _candidates.FirstOrDefault();
    }

    public void AddCharBlackList(params char[] blockChars)
    {
        _candidates.RemoveAll(w => w.Any(c => blockChars.Contains(c)));
    }

    public void AddConfirmChar(char c, int position)
    {
        _candidates.RemoveAll(w => w[position] != c);
    }

    public void AddNotInpositionChar(char c, int position)
    {
        _candidates.RemoveAll(w => w[position] == c || !w.Contains(c));
    }

    public void RemoveCandidates(string v)
    {
        _candidates.RemoveAll(w => w == v);
    }
}