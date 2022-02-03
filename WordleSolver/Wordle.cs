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

    public GuessResult Guess(string inputGuess)
    {
        if (_answer == null)
        {
            throw new InvalidOperationException();
        }

        var charResults = inputGuess.Select((c, inputPosition) =>
        {
            if (_answer[inputPosition] == c)
            {
                return new GuessResult.GuessCharResult(c, GuessResult.GuessCharType.Match);
            }
            else if (_answer.Where((answerChar,answerPosition) => answerChar == c 
                && answerPosition != inputPosition 
                && inputGuess[answerPosition] != c).Any())
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
