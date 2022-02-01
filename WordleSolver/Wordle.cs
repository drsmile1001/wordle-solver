namespace WordleSolver;

public class Wordle
{
    private string? _answer;

    public void SetAnswer(string v)
    {
        if (v.Length != 5)
        {
            throw new ArgumentOutOfRangeException(nameof(v));
        }
        _answer = v;
    }

    public GuessResult Guess(string v)
    {
        if (_answer == null)
        {
            throw new InvalidOperationException();
        }

        var charResults = v.Select((c, i) =>
        {
            if (_answer[i] == c)
            {
                return new GuessResult.GuessCharResult(c, GuessResult.GuessCharType.Match);
            }
            else if (_answer.Contains(c))
            {
                return new GuessResult.GuessCharResult(c, GuessResult.GuessCharType.NotInPosition);
            }
            else
            {
                return new GuessResult.GuessCharResult(c, GuessResult.GuessCharType.None);
            }
        }).ToArray();

        return new GuessResult(charResults);
    }
}
