using FluentAssertions;
using Xunit;

namespace WordleSolver.Test;

public class WordSetTest
{
    [Fact]
    public void 取得特定字母數量_字母存在_得到數量()
    {
        var words = new[]
        {
            "abcde",
            "abfgh"
        };
        var wordSet = new WordSet(words);

        var charACount = wordSet.GetCharCount('a');

        charACount.Should().Be(2);
    }

    [Fact]
    public void 取得特定字母數量_字母不存在_得到0()
    {
        var words = new[]
        {
            "abcde",
            "abfgh"
        };
        var wordSet = new WordSet(words);

        var charICount = wordSet.GetCharCount('i');

        charICount.Should().Be(0);
    }

    [Fact]
    public void 取得特定字母在特定位置數量_字母存在_得到數量()
    {
        var words = new[]
        {
            "abcde",
            "abfgh",
            "bcdea"
        };
        var wordSet = new WordSet(words);
            
        var charACountAtPosition0 = wordSet.GetCharCountAtPosition('a', 0);
        var charACountAtPosition4 = wordSet.GetCharCountAtPosition('a', 4);

        charACountAtPosition0.Should().Be(2);
        charACountAtPosition4.Should().Be(1);
    }

    [Fact]
    public void 取得特定字母在特定位置數量_字母不存在_得到0()
    {
        var words = new[]
        {
            "abcde",
            "abfgh"
        };
        var wordSet = new WordSet(words);

        var charICountAtPosition0 = wordSet.GetCharCountAtPosition('i', 0);

        charICountAtPosition0.Should().Be(0);
    }
}
