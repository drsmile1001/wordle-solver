namespace WordleSolver;

public class GuessResult : IEquatable<GuessResult>
{
    public GuessResult(GuessCharResult[] guessCharResults)
    {
        GuessCharResults = guessCharResults;
    }

    public IReadOnlyList<GuessCharResult> GuessCharResults { get; }

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

    public override string ToString()
    {
        return GuessCharResults
            .Select(item => item.ToString())
            .Aggregate((current, next) => $"{current} {next}");
    }

    public bool IsMatch(string word)
    {
        for (int position = 0; position < 5; position++)
        {
            var rule = GuessCharResults[position];
            switch (rule.Type)
            {
                case GuessCharType.Match:
                    if(word[position]!= rule.Char) return false;
                    break;
                case GuessCharType.NotInPosition:
                    if (word[position] == rule.Char) return false;
                    for (int wordPosition = 0; wordPosition < 5; wordPosition++)
                    {
                        var wordChar = word[wordPosition];
                        if(wordChar == rule.Char && wordPosition != position && GuessCharResults[wordPosition].Char != wordChar) return true;
                    }
                    return false;
                case GuessCharType.None:
                    if (word[position] == rule.Char) return false;
                    for (int wordPosition = 0; wordPosition < 5; wordPosition++)
                    {
                        var wordChar = word[wordPosition];
                        if(wordChar == rule.Char)
                        {
                            var guessCharResult = GuessCharResults[wordPosition];
                            if(guessCharResult.Char != wordChar || guessCharResult.Type != GuessCharType.Match) return false;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        return true;
    }

    public enum GuessCharType
    {
        Match,
        NotInPosition,
        None,
    }

    public record GuessCharResult(char Char, GuessCharType Type)
    {
        public override string ToString()
        {
            var sym = Type switch
            {
                GuessCharType.Match => "+",
                GuessCharType.NotInPosition => "*",
                GuessCharType.None => "-",
                _ => throw new NotImplementedException(),
            };
            return $"{Char}{sym}";
        }
    }
}