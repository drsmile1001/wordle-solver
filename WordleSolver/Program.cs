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
    throw new NotImplementedException();
}


