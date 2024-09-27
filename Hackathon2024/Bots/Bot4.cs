using Hackathon_24;
using Hackathon2024.DTO;
using Hackathon2024.SqlHelper;

namespace Hackathon2024.Bots ;

    public class Bot4 : IBot
    {
        private readonly PossibleWordList _possibleWordList;

        private List<char> _probabilities =
            LettersToPercentage.GetLettersToPercentage()
                               .OrderByDescending(x => x.Value)
                               .Select(x => x.Key)
                               .ToList();

        public string BotName { get; }

        public Bot4(PossibleWordList possibleWordList,
            string botName)
        {
            _possibleWordList = possibleWordList;
            BotName = botName;
        }

        public char CalculateNextStep(Round round)
        {
            List<string> possibleWords = new List<string>();
            if (round.guessed.Count > 0)
            {
                var wrongGuesses = _possibleWordList.GetWrongGuesses(round.word, round.guessed);
                possibleWords = _possibleWordList.GetPossibleWordList(round.word, wrongGuesses);
            }
            else
            {
                return 'E';
            }

            return Calculation.CalculateMostLikelyWord(possibleWords, round.guessed);
        }

    public int Complete(Result result)
    {
        AddNewWord.AddWordToDatabase(result.word!);
        BotResultTextWriter.WriteText(result.word!, BotName);
        return 0;
    }
}