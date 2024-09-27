namespace Hackathon2024 ;

    public sealed class BotFactory
    {
        public static IBot CreateBot(string botName)
            => botName switch{
                _ or "JackyOne" => new Bot1(botName)
                };
    }