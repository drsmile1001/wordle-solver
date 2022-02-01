using Sharprompt;
using System.Text.RegularExpressions;
using WordleSolver;


var functionName = Prompt.Select<string>(c =>
{
    c.Message = "選擇功能";
    c.Items = new[] 
    {
        "解題",
        "基準測試",
    };
});

switch (functionName)
{
    case "解題":
        Resolve();
        break;
    case "基準測試":
        BenchMark();
        break;
    default:
        break;
}

void Resolve()
{
    var wordTable = new WordTable();
    var blockWordTable = new BlockWordTable();
    var game = new Game(wordTable, blockWordTable);

    while (true)
    {
        var nextGuess = game.NextGuess();
        if (nextGuess == null)
        {
            Console.WriteLine("找不到符合的字");
            break;
        }
        Console.WriteLine($"本次猜測:{nextGuess}");

        var inWordList = Prompt.Confirm("是否有結果？", defaultValue:true);

        if(!inWordList)
        {
            game.RemoveCandidates(nextGuess);
            continue;
        }

        var isMatch = Prompt.Confirm("是否已命中？", defaultValue: false);
        if(isMatch)
        {
            break;
        }

        for (int position = 0; position < nextGuess.Length; position++)
        {
            var c = nextGuess[position];
            var r = Prompt.Select<GuessResult.GuessCharType>($"第 {position + 1} 字母 {c} 結果");
            switch (r)
            {
                case GuessResult.GuessCharType.Match:
                    game.AddConfirmChar(c, position);
                    break;
                case GuessResult.GuessCharType.NotInPosition:
                    game.AddNotInpositionChar(c, position);
                    break;
                case GuessResult.GuessCharType.None:
                    game.AddCharBlackList(c);
                    break;
                default:
                    break;
            }
        }
    }

    Console.WriteLine("按任何鍵結束");
    Console.ReadKey();
}


void BenchMark()
{
    var wordTable = new WordTable();
    var words = wordTable.GetWords();
    var blackTable = new BlockWordTable();
    var blcokWordHashSet = new HashSet<string>(blackTable.GetBlockWords());
    Regex _validWordPattern = new("^[a-z]{5}$");
    var rnd = new Random();
    var guessTimes = words
        .Select(x => x.ToLower().Trim())
        .Where(x => x.Length == 5)
        .Where(w => _validWordPattern.IsMatch(w) && !blcokWordHashSet.Contains(w))
        .OrderBy(x => rnd.Next())
        .Take(1000)
        .AsParallel()
        .Select(word =>
        {
            var wordle = new Wordle();
            wordle.SetAnswer(word);
            var game = new Game(wordTable, blackTable);
            var guessTimes = 1;
            for (; ; guessTimes++)
            {
                var guess = game.NextGuess()!;
                var result = wordle.Guess(guess);
                if(result.GuessCharResults.All(r =>r.Type  == GuessResult.GuessCharType.Match))
                {
                    break;
                }

                for (int position = 0; position < guess.Length; position++)
                {
                    var charResult = result.GuessCharResults[position];
                    var c = charResult.Char;
                    switch (charResult.Type)
                    {
                        case GuessResult.GuessCharType.Match:
                            game.AddConfirmChar(c, position);
                            break;
                        case GuessResult.GuessCharType.NotInPosition:
                            game.AddNotInpositionChar(c, position);
                            break;
                        case GuessResult.GuessCharType.None:
                            game.AddCharBlackList(c);
                            break;
                        default:
                            break;
                    }
                }
            }

            return guessTimes;
        }).ToArray();

    Console.WriteLine($"總猜測次數: {guessTimes.Sum()}");
    Console.WriteLine($"平均猜測次數: {guessTimes.Average()}");
    Console.WriteLine($"最大猜測次數: {guessTimes.Max()}");
}


