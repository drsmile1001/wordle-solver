using System.Text.RegularExpressions;

namespace WordleSolver;

public class Solver
{
    private readonly List<string> _candidates = new();
    private readonly Regex _validWordPattern = new("^[a-z]{5}$");
    private readonly IBlockWordTable _blockWordTable;
    private readonly List<char> _noneChars = new();
    private readonly List<(char c, int Position)> _confirmChars = new();
    private readonly List<(char c, int Position)> _notInPositionChars = new();
    private readonly List<string> _guessed = new();
    private WordSet? _wordSet;

    public Solver(IWordTable wordTable, IBlockWordTable blockWordTable)
    {
        var blcokWordHashSet = new HashSet<string>(blockWordTable.GetBlockWords());
        _candidates = wordTable.GetWords()
            .Where(w => w.Length == 5)
            .Select(w => w.ToLower())
            .Where(w => _validWordPattern.IsMatch(w) && !blcokWordHashSet.Contains(w))
            .ToList();

        _blockWordTable = blockWordTable;
    }

    public string? NextGuess()
    {
        string[] matchCandidates = GetMatchedWords();

        if (matchCandidates.Length == 0)
        {
            return null;
        }

        if (matchCandidates.Length == 1)
        {
            return matchCandidates[0];
        }

        _wordSet = new WordSet(matchCandidates);

        var guess = _candidates.Where(word => !_guessed.Contains(word))
            .OrderByDescending(word => GetWordWeight(word))
            .FirstOrDefault();
        if (guess != null)
            _guessed.Add(guess);
        return guess;
    }

    private string[] GetMatchedWords()
    {
        return _candidates.Where(word =>
        {
            foreach (var (c, position) in _confirmChars)
            {
                if (word[position] != c)
                {
                    return false;
                }
            }

            foreach (var (c, position) in _notInPositionChars)
            {
                if (word[position] == c || !word.Contains(c))
                {
                    return false;
                }
            }

            if (word.Intersect(_noneChars).Any())
            {
                return false;
            }
            return true;
        })
        .Where(c => !_guessed.Contains(c))
        .ToArray();
    }

    private int GetWordWeight(string word)
    {
        var wordWeight = 0;

        for (int position = 0; position < word.Length; position++)
        {
            var c = word[position];
            wordWeight += _wordSet!.GetCharCountAtPosition(c, position);
        }

        if (word.GroupBy(c => c).Count() != 5)
            wordWeight = (int)(wordWeight * 0.9);

        return wordWeight;
    }

    public void AddNoneChars(params char[] noneChars)
    {
        _noneChars.AddRange(noneChars);
    }

    public void AddConfirmChar(char c, int position)
    {
        _confirmChars.Add(new (c,position));
    }

    public void AddNotInpositionChar(char c, int position)
    {
        _notInPositionChars.Add(new (c,position));
    }

    public void RemoveCandidates(string word)
    {
        _blockWordTable.AddWord(word);
        _candidates.RemoveAll(w => w == word);
    }
}
