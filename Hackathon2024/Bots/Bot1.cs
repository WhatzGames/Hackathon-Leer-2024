using Hackathon2024.DTO;
using Hackathon2024.SqlHelper;

namespace Hackathon2024.Bots ;

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
            char selected = ALPHABET[Random.Shared.Next(0, ALPHABET.Length)];
            while (round.guessed!.Contains(selected))
            {
                selected = ALPHABET[Random.Shared.Next(0, ALPHABET.Length)];
            }
            return selected;
        }

        public int Complete(Result result)
        {
            AddNewWord.AddWordToDatabase(result.word!);
            BotResultTextWriter.WriteText(result.word!, BotName);
            return 0;
        }
    }