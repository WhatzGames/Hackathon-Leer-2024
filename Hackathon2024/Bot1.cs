using Hackathon2024.DTO;

namespace Hackathon2024 ;

    public sealed class Bot1 : IBot
    {
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string BotName { get; }

        public Bot1(string botName)
        {
            BotName = botName;
        }

        public char CalculateNextStep(Round round)
        {
            return ALPHABET[Random.Shared.Next(0, ALPHABET.Length)];
        }

        public int Complete(Result result)
        {
            return 0;
        }
    }