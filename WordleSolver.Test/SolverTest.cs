using System;
using FluentAssertions;
using Xunit;

namespace WordleSolver.Test;

public class SolverTest
{
    [Fact]
    public void 取得推薦猜測_沒有前提_取得符合遊戲規範的字串()
    {
        var wordTable = new FakeWordTable(new[]
        {
            "123",
            "ABC",
            "abcdef",
            "abcde"
        });
        var blockTable = new FakeBlockTable(Array.Empty<string>());
        var game = new Solver(wordTable, blockTable);

        var s = game.NextGuess();

        GuessWordShouldBeVaild(s);
    }

    [Fact]
    public void 取得推薦猜測_輸入過黑名單且可用以推斷唯一解_取得解答()
    {
        var wordTable = new FakeWordTable(new[]
        {
            "abcde",
            "efghi",
            "fghij",
        });
        var blockTable = new FakeBlockTable(Array.Empty<string>());
        var game = new Solver(wordTable, blockTable);
        game.AddNoneChars('a', 'b', 'c', 'd', 'e');

        var s = game.NextGuess();
        s.Should().Be("fghij");
    }

    [Fact]
    public void 取得推薦猜測_輸入過確定位置字母且可利用推斷唯一解_取得解答()
    {
        var wordTable = new FakeWordTable(new[]
        {
            "abcde",
            "efghi",
            "fghij",
        });
        var blockTable = new FakeBlockTable(Array.Empty<string>());
        var game = new Solver(wordTable, blockTable);
        game.AddConfirmChar('e', 0);

        var s = game.NextGuess();
        s.Should().Be("efghi");
    }

    [Fact]
    public void 取得推薦猜測_輸入不確定位置字母且可用以推斷唯一解_取得解答()
    {
        var wordTable = new FakeWordTable(new[]
        {
            "abcde",
            "bacde",
            "efghi",
        });
        var blockTable = new FakeBlockTable(Array.Empty<string>());
        var game = new Solver(wordTable, blockTable);
        game.AddNotInpositionChar('e', 4);

        var s = game.NextGuess();
        s.Should().Be("efghi");
    }

    [Fact]
    public void 取得推薦猜測_刪除候選字_取得不是候選字的其他字()
    {
        var wordTable = new FakeWordTable(new[]
        {
            "abcde",
            "bacde",
            "efghi",
        });
        var blockTable = new FakeBlockTable(Array.Empty<string>());
        var game = new Solver(wordTable, blockTable);
        game.RemoveCandidates("abcde");

        var s = game.NextGuess();
        s.Should().Be("bacde");
    }

    [Fact]
    public void 取得推薦猜測_有刪除字記錄_取得不是刪除字的其他字()
    {
        var wordTable = new FakeWordTable(new[]
        {
            "abcde",
            "bacde",
            "efghi",
        });
        var blockTable = new FakeBlockTable(new[]{
            "abcde",
        });
        var game = new Solver(wordTable, blockTable);

        var s = game.NextGuess();
        s.Should().Be("bacde");
    }

    private static void GuessWordShouldBeVaild(string? s)
    {
        s.Should().NotBeNullOrWhiteSpace(s);
        s.Should().HaveLength(5);
        s.Should().MatchRegex("^[a-z]{5}$");
    }
}