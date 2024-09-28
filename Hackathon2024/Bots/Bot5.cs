using Hackathon2024.DTO;
using Hackathon2024.SqlHelper;

namespace Hackathon2024.Bots ;

    public sealed class Bot5 : IBot
    {
        private readonly PossibleWordList _possibleWordList;

        public Bot5(PossibleWordList possibleWordList)
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
            if (hits is 0)
            {
                possibleWords = _possibleWordList.GetPossibleWordList(wrongGuesses);
            }

            (character, hits) = Calculation.GetHits(possibleWords, round.guessed);

            return character;
        }

        public int Complete(Result result)
        {
            return 0;
        }
    }