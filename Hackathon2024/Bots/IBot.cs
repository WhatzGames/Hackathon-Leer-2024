using Hackathon2024.DTO;

namespace Hackathon2024.Bots ;

    public interface IBot
    {
        public string BotName { get; }
        char CalculateNextStep(Round round);
        int Complete(Result result);
    }