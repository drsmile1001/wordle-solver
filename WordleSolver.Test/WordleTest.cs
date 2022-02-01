using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace WordleSolver.Test;

public class WordleTest
{
    [Theory]
    [MemberData(nameof(輸入猜測_猜測是有效字_得到猜測結果_data))]
    public void 輸入猜測_猜測是有效字_得到猜測結果(string answer, string guess, GuessResult expectGuessResult)
    {
        var wordle = new Wordle();
        wordle.SetAnswer(answer);

        var guessResult = wordle.Guess(guess);

        guessResult.Should().Be(expectGuessResult);
    }

    public static IEnumerable<object[]> 輸入猜測_猜測是有效字_得到猜測結果_data
    => new List<object[]>
    {
        new object[]{ "abcde","abcde", new GuessResult(new[]
        {
            new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.Match),
            new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.Match),
            new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.Match),
            new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.Match),
            new GuessResult.GuessCharResult('e',GuessResult.GuessCharType.Match),
        })},
        new object[]{ "abcde","bcdea", new GuessResult(new[]
        {
            new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.NotInPosition),
            new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.NotInPosition),
            new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.NotInPosition),
            new GuessResult.GuessCharResult('e',GuessResult.GuessCharType.NotInPosition),
            new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
        })},
        new object[]{ "abcde","fghij", new GuessResult(new[]
        {
            new GuessResult.GuessCharResult('f',GuessResult.GuessCharType.None),
            new GuessResult.GuessCharResult('g',GuessResult.GuessCharType.None),
            new GuessResult.GuessCharResult('h',GuessResult.GuessCharType.None),
            new GuessResult.GuessCharResult('i',GuessResult.GuessCharType.None),
            new GuessResult.GuessCharResult('j',GuessResult.GuessCharType.None),
        })},
    };
}