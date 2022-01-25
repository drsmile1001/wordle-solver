using System.Text.RegularExpressions;
using WordleSolver;

var wordTable = new WordTable();
var blockWordTable = new BlockWordTable();
var game = new Game(wordTable, blockWordTable);

while (true)
{
    Console.WriteLine(string.Empty);
    Console.WriteLine("請輸入指令");
    var command = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(command))
    {
        Console.WriteLine("未確認到有效指令");
        continue;
    }

    if (command == "exit")
    {
        Console.WriteLine("指令:結束");
        break;
    }

    if (command == "guess")
    {
        Console.WriteLine("指令:猜測");
        var nextGuess = game.NextGuess();
        if (nextGuess == null)
        {
            Console.WriteLine("找不到符合的字");
        }
        Console.WriteLine(nextGuess);
        continue;
    }

    if (command.StartsWith("result "))
    {
        Console.WriteLine("指令:輸入猜測結果");
        var regex = new Regex(@"(?<c>[a-z])(?<t>[\+\-\*])");
        var results = command["result ".Length..];

        var matches = regex.Matches(results);

        if (matches.Count != 5)
        {
            Console.WriteLine("輸入猜測結果指令格式錯誤");
            continue;
        }
        for (var position = 0; position < 5; position++)
        {
            var match = matches[position];

            var c = match.Groups["c"].Value[0];
            var t = match.Groups["t"].Value;
            switch (t)
            {
                case "+":
                    game.AddConfirmChar(c, position);
                    break;
                case "-":
                    game.AddCharBlackList(c);
                    break;
                case "*":
                    game.AddNotInpositionChar(c, position);
                    break;
                default:
                    break;
            }
        }
        continue;
    }


    if (command.StartsWith("remove "))
    {
        Console.WriteLine("指令:刪除候選字");
        var word = command["remove ".Length..];
        game.RemoveCandidates(word);
        continue;
    }
    Console.WriteLine("未確認到有效指令");
    continue;
}