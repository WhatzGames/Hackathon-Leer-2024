using Hackathon2024.DTO;

namespace Hackathon2024.Bots ;

    public interface IBot
    {
        char CalculateNextStep(Round round);
        int Complete(Result result);
    }