using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WordleSolver;

public class Solver
{
    private readonly List<string> _candidates = new();
    private readonly Regex _validWordPattern = new("^[a-z]{5}$");
    private readonly IBlockWordTable _blockWordTable;
    private readonly List<GuessResult> _guessed = new();


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
        string[] matchedCandidates = MatchWordWithFilters(_candidates, _guessed);
        Console.WriteLine($"候選字數量:{matchedCandidates.Length}");
        if (matchedCandidates.Length == 0)
        {
            return null;
        }

        if (matchedCandidates.Length == 1)
        {
            return matchedCandidates[0];
        }

        var guess = _candidates
            .AsParallel()
            .Select(word =>
            {
                var r = new
                {
                    word,
                    avgFilter = GetWordAverageFilteringLast(word, matchedCandidates)
                };
                return r;
            })
            .OrderBy(item => item.avgFilter)
            .FirstOrDefault();
        if (guess == null) return null;
        Console.WriteLine($"推薦候選字{guess.word} 推估可縮減候選字數量至{guess.avgFilter}");
        return guess.word;
    }

    private static string[] MatchWordWithFilters(IEnumerable<string> words, IEnumerable<GuessResult> filters)
    {
        var result = words.Where(w => filters.All(f => f.IsMatch(w))).ToArray();
        return result;
    }

    /// <summary>
    /// 計算某單字平均可以篩剩下的單字數量
    /// </summary>
    /// <param name="word"></param>
    /// <param name="matchedCandidates"></param>
    /// <returns></returns>
    private double GetWordAverageFilteringLast(string word, string[] matchedCandidates)
    {
        var filterResult = new Dictionary<GuessResult, int>();

        var sw = Stopwatch.StartNew();

        var results = new List<int>();

        foreach (var answer in matchedCandidates.OrderBy(w => Guid.NewGuid()))
        {
            var wordle = new Wordle();
            wordle.SetAnswer(answer);
            var filter = wordle.Guess(word);
            if (filterResult.TryGetValue(filter, out var cachedValue))
            {
                results.Add(cachedValue);
            }
            else
            {
                var newValue = MatchWordWithFilters(matchedCandidates, new[] { filter }).Length;
                filterResult.Add(filter, newValue);
                results.Add(newValue);
            }
            if (sw.ElapsedMilliseconds > 20) break;
        }
        return results.Average();
    }

    public void AddGuessResult(GuessResult guessResult)
    {
        _guessed.Add(guessResult);
    }

    public void RemoveCandidates(string word)
    {
        _blockWordTable.AddWord(word);
        _candidates.RemoveAll(w => w == word);
    }
}
