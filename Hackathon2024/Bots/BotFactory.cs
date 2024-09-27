using Hackathon2024.SqlHelper;

namespace Hackathon2024.Bots ;

    public sealed class BotFactory
    {
        public static IBot CreateBot(string botName)
            => botName switch{
                "JackyOne" => new Bot1(botName),
                "JackyTwo" => new Bot2(botName),
                "JackyThree" =>
                    new Bot3(botName,
                        new FileInfo(Environment.CurrentDirectory + "/Hackathon2024/sanitizedwortliste.txt")),
                "JackyFour" => new Bot4(new PossibleWordList(), botName),

                _ => new Bot1(botName),
                };
    }