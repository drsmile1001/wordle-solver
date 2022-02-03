using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace WordleSolver.Test;

public class GuessResultTest
{
    [Fact]
    public void ToStringTest()
    {
        var gs = new GuessResult(new[]
        {
                new GuessResult.GuessCharResult('a', GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('b', GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('c', GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d', GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('e', GuessResult.GuessCharType.Match),
            });

        gs.ToString().Should().Be("a+ b* c- d* e+");
    }

    [Theory]
    [MemberData(nameof(檢查單字是否符合猜測結果_data))]
    public void 檢查單字是否符合猜測結果(GuessResult filter, string word, bool expect)
    {
        var result = filter.IsMatch(word);

        result.Should().Be(expect);
    }

    public static IEnumerable<object[]> 檢查單字是否符合猜測結果_data
    => new List<object[]>
    {
        new object[]{ 
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('e',GuessResult.GuessCharType.Match),
            }),
            "abcde",
            true
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('e',GuessResult.GuessCharType.Match),
            }),
            "bcdea",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('e',GuessResult.GuessCharType.Match),
            }),
            "fghij",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('e',GuessResult.GuessCharType.None),
            }),
            "abcde",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('e',GuessResult.GuessCharType.None),
            }),
            "bcdea",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('e',GuessResult.GuessCharType.None),
            }),
            "fghij",
            true
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('e',GuessResult.GuessCharType.NotInPosition),
            }),
            "abcde",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('e',GuessResult.GuessCharType.NotInPosition),
            }),
            "bcdea",
            true
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('e',GuessResult.GuessCharType.NotInPosition),
            }),
            "fghij",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "aeafh",
            true
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "aaafh",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "aefgh",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "aefgh",
            true
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "aefga",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.Match),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "aafgh",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "afghi",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "faghi",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "efghi",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "efgha",
            true
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.NotInPosition),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "efgaa",
            true
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "efghi",
            true
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "afghi",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "eaghi",
            false
        },
        new object[]{
            new GuessResult(new[]
            {
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('a',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('b',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('c',GuessResult.GuessCharType.None),
                new GuessResult.GuessCharResult('d',GuessResult.GuessCharType.None),
            }),
            "efgha",
            false
        },
    };
}
