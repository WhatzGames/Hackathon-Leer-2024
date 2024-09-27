using Hackathon2024.DTO;

namespace Hackathon2024.Bots ;

    public sealed class Bot3 : IBot
    {
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly HashSet<string> _words;
        public string BotName { get; }

        public Bot3(string botName, FileInfo fileInfo)
        {
            BotName = botName;
            _words = [];
            StreamReader reader = new(fileInfo.OpenRead());
            while (reader.ReadLine() is {} line)
            {
                _words.Add(line.ToUpper());
            }
        }

        public char CalculateNextStep(Round round)
        {
            if (round.guessed is null or {Count: 0} || round.word is null)
            {
                char selected = ALPHABET[Random.Shared.Next(0, ALPHABET.Length)];
                while (round.guessed!.Contains(selected))
                {
                    selected = ALPHABET[Random.Shared.Next(0, ALPHABET.Length)];
                }
                return selected;
            }
            char last = round.guessed.Last();
            int matches = round.word.Count(x => x == last);
            _words.RemoveWhere(x => x.Count(y => y == last) != matches);
            return Calculation.CalculateMostLikelyWord(_words, round.guessed);
        }


        public int Complete(Result result)
        {
            return 0;
        }
    }