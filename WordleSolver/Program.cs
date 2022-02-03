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
    var game = new Solver(wordTable, blockWordTable);

    while (true)
    {
        var guess = game.NextGuess();
        if (guess == null)
        {
            Console.WriteLine("找不到符合的字");
            break;
        }
        Console.WriteLine($"本次猜測:{guess}");

        var inWordList = Prompt.Confirm("是否能輸入猜測？", defaultValue:true);

        if(!inWordList)
        {
            game.RemoveCandidates(guess);
            continue;
        }

        var isMatch = Prompt.Confirm("是否已命中？", defaultValue: false);
        if(isMatch)
        {
            break;
        }

        var charResults = guess.Select((c,position)=>
        {
            var r = Prompt.Select<GuessResult.GuessCharType>($"第 {position + 1} 字母 {c} 結果");
            return new GuessResult.GuessCharResult(c,r);
        }).ToArray();

        game.AddGuessResult(new GuessResult(charResults));
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
        .Take(10)
        .Select(word =>
        {
            var wordle = new Wordle();
            wordle.SetAnswer(word);
            Console.WriteLine($"謎底:{word}");
            var game = new Solver(wordTable, blackTable);
            var guessTimes = 1;
            for (; ; guessTimes++)
            {
                if (guessTimes > 20)
                    throw new Exception($"猜測 {word} 次數大於限制");
                var guess = game.NextGuess()!;
                var result = wordle.Guess(guess);
                Console.WriteLine($"猜測:{guess} 結果:{result}");
                if (result.GuessCharResults.All(r =>r.Type  == GuessResult.GuessCharType.Match))
                {
                    break;
                }

                game.AddGuessResult(result);
            }

            return guessTimes;
        }).ToArray();

    Console.WriteLine($"總猜測次數: {guessTimes.Sum()}");
    Console.WriteLine($"平均猜測次數: {guessTimes.Average()}");
    Console.WriteLine($"最大猜測次數: {guessTimes.Max()}");
}


