namespace WordleSolver;

public class GuessResult : IEquatable<GuessResult>
{
    public GuessResult(GuessCharResult[] guessCharResults)
    {
        GuessCharResults = guessCharResults;
    }


    public GuessCharResult[] GuessCharResults { get; }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        foreach (var guessCharResult in GuessCharResults)
        {
            hashCode.Add(guessCharResult.GetHashCode());
        }
        return hashCode.ToHashCode();
    }

    public bool Equals(GuessResult? other)
    {
        if(other == null) return false;
        return GuessCharResults.SequenceEqual(other.GuessCharResults);
    }

    public override bool Equals(object? obj)
    {
        if(obj is not GuessResult) return false;
        return Equals((GuessResult)obj);
    }

    public enum GuessCharType
    {
        Match,
        NotInPosition,
        None,
    }

    public record GuessCharResult(char Char, GuessCharType Type);
}