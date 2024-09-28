using Hackathon_24;

namespace Hackathon2024 ;

    public static class Calculation
    {
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static char CalculateMostLikelyWord(IEnumerable<string> words, List<char> guessedCharacters)
        {
            Dictionary<char, double> probabilities = ALPHABET.Except(guessedCharacters)
                                                             .Select(x => KeyValuePair.Create(x, 0d))
                                                             .ToDictionary();
            double wordCount = 0;
            foreach (string word in words)
            {
                wordCount += 1;
                HashSet<char> chars = [];
                foreach (char c in word)
                {
                    if (chars.Add(c) && probabilities.TryGetValue(c, out double _))
                    {
                        probabilities[c] += 1;
                    }
                }
            }

            if (wordCount is 0)
            {
                Dictionary<char, double> staticWeights = LettersToPercentage.GetLettersToPercentage();
                guessedCharacters.ForEach(x => staticWeights.Remove(x));
                return staticWeights.OrderByDescending(x => x.Value)
                                    .Select(x => x.Key)
                                    .First();
            }

            return probabilities.OrderByDescending(x => x.Value)
                                .Select(x => x.Key)
                                .First();
        }

        public static KeyValuePair<char, int> GetHits(IEnumerable<string> words, List<char> guessedCharacters)
        {
            Dictionary<char, int> hits = ALPHABET.Except(guessedCharacters)
                                                 .Select(x => KeyValuePair.Create(x, 0))
                                                 .ToDictionary();
            foreach (string word in words)
            {
                HashSet<char> chars = [];
                foreach (char c in word)
                {
                    if (chars.Add(c) && hits.ContainsKey(c))
                    {
                        hits[c] += 1;
                    }
                }
            }
            return hits.OrderByDescending(x => x.Value)
                       .First();
        }

        public static char RandomLeftOver(IEnumerable<char> guessedCharacters)
        {
            var leftovers = ALPHABET.Except(guessedCharacters).ToArray();
            Random.Shared.Shuffle(leftovers);
            return leftovers.First();
        }
    }