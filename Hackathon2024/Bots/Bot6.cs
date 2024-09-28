using Hackathon2024.DTO;
using Hackathon2024.SqlHelper;

namespace Hackathon2024.Bots ;

    public sealed class Bot6 : IBot
    {
        private readonly PossibleWordList _possibleWordList;
        private char? randomlyGuessedChar;
        public string BotName { get; }

        public Bot6(PossibleWordList possibleWordList, string botName)
        {
            _possibleWordList = possibleWordList;
        }
        public char CalculateNextStep(Round round)
        {
            if (round.guessed.Count is 0)
            {
                return 'E';
            }

            var wrongGuesses = _possibleWordList.GetWrongGuesses(round.word, round.guessed);
            List<string> possibleWords = _possibleWordList.GetPossibleWordList(round.word, wrongGuesses);

            (char character, int hits) = Calculation.GetHits(possibleWords, round.guessed);

            if (randomlyGuessedChar is null)
            {                
                randomlyGuessedChar = Calculation.RandomLeftOver(round.guessed);
                return randomlyGuessedChar.Value;
            }
            
            if (hits is 0)
            {
                possibleWords = _possibleWordList.GetPossibleWordList(wrongGuesses);
            }

            (character, hits) = Calculation.GetHits(possibleWords, round.guessed);
            

            return character;
        }

        public int Complete(Result result)
        {
            AddNewWord.AddWordToDatabase(result.word!);
            BotResultTextWriter.WriteText(result.word!, BotName);
                return 0;
        }
    }