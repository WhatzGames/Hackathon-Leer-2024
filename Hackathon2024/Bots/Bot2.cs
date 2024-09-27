using Hackathon_24;
using Hackathon2024.DTO;
using Hackathon2024.SqlHelper;

namespace Hackathon2024.Bots ;

    public class Bot2 : IBot
    {
        private List<char> _probabilities =
            LettersToPercentage.GetLettersToPercentage()
                               .OrderByDescending(x => x.Value)
                               .Select(x => x.Key)
                               .ToList();

        private int counter = -1;
        public string BotName { get; }

        public Bot2(string botName)
        {
            BotName = botName;
        }

        public char CalculateNextStep(Round round)
        {
            counter++;
            if (counter == _probabilities.Count)
            {
                return '*';
            }
            return _probabilities[counter];
        }

        public int Complete(Result result)
        {
            AddNewWord.AddWordToDatabase(result.result!);
            return 0;
        }
    }