using FluentAssertions;
using Xunit;

namespace WordleSolver.Test;

public class GameTest
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
        var game = new Game(wordTable);

        var s = game.NextGuess();

        GuessWordShouldBeVaild(s);
    }

    [Fact]
    public void 取得推薦猜測_字典有重複字母字_優先取得無重複字母的字()
    {
        var wordTable = new FakeWordTable(new[]
        {
            "aaabc",
            "aabcd",
            "abcde",
        });
        var game = new Game(wordTable);

        var s = game.NextGuess();

        s.Should().Be("abcde");
    }

    [Fact]
    public void 取得推薦猜測_輸入過黑名單_取得不在黑名單的字串()
    {
        var wordTable = new FakeWordTable(new[]
        {
            "abcde",
            "efghi",
            "fghij",
        });
        var game = new Game(wordTable);
        game.AddCharBlackList('a', 'b', 'c', 'd', 'e');

        var s = game.NextGuess();
        s.Should().Be("fghij");
    }

    [Fact]
    public void 取得推薦猜測_輸入過確定位置字母_取得有符合確定字母的字串()
    {
        var wordTable = new FakeWordTable(new[]
        {
            "abcde",
            "efghi",
            "fghij",
        });
        var game = new Game(wordTable);
        game.AddConfirmChar('e', 0);

        var s = game.NextGuess();
        s.Should().Be("efghi");
    }

    [Fact]
    public void 取得推薦猜測_輸入不確定位置字母_取得有在其他位置包含字母的字串()
    {
        var wordTable = new FakeWordTable(new[]
        {
            "abcde",
            "bacde",
            "efghi",
        });

        var game = new Game(wordTable);
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

        var game = new Game(wordTable);
        game.RemoveCandidates("abcde");

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