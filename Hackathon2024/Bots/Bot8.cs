using Hackathon_24;
using Hackathon2024.DTO;
using Hackathon2024.SqlHelper;
using Microsoft.Data.Sqlite;

namespace Hackathon2024.Bots;

public class Bot8 : IBot
{
    public string BotName => "JackyEight";
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public char CalculateNextStep(Round round)
    {
        var word = round.word ?? string.Empty;
        var guessed = round.guessed ?? [];
        var failedChars = guessed.Except(word.ToCharArray()).ToList();
        
        Console.WriteLine($"Round; word: {word}; guessed: {string.Concat(guessed)}; failed: {string.Concat(failedChars)}");
        int firstGuessedCharIndex = GetFirstLetterIndex(word);
        if (firstGuessedCharIndex == -1)
        {
            var chosenStandardLetter = GetStandardProbabilityLetter(guessed);
            Console.WriteLine($"Standard chosen letter: {chosenStandardLetter}");
            return chosenStandardLetter;
        }
        
        int lastGuessedCharIndex = GetLastLetterIndext(word);
        
        if (lastGuessedCharIndex == -1)
        {
            var chosenStandardLetter = GetStandardProbabilityLetter(guessed);
            Console.WriteLine($"Standard chosen letter: {chosenStandardLetter}");
            return chosenStandardLetter;
        }
        
        var fullSearchWord = word.Substring(firstGuessedCharIndex,lastGuessedCharIndex- firstGuessedCharIndex+1 );
        
        
        var fullSearchResultCharList =  GetFullSearchResultCharList(word, fullSearchWord, failedChars, guessed);

        for (int i = 0; i < fullSearchWord.Length; i++)
        {
            
        }
        
        var chosenLetter = fullSearchResultCharList.First();
        Console.WriteLine(chosenLetter);

        return chosenLetter;
    }

    private char GetStandardProbabilityLetter(List<char> guessed)
    {
       return LettersToPercentage.GetLettersToPercentage().ExceptBy(guessed, x => x.Key)
            .OrderByDescending(x => x.Value)
            .Select(x => x.Key)
            .First();
    }

    private List<char> GetFullSearchResultCharList(string word, string fullSearchWord,
        List<char> failedChars, List<char> guessed)
    {
        var fullSearchResultIndex = CreateFullSearchResultIndex(fullSearchWord, failedChars, guessed);
        return fullSearchResultIndex.OrderByDescending(x => x.Value).Take(5).Select(x => x.Key)
            .ToList();
    }

    private Dictionary<char, int> CreateFullSearchResultIndex(string fullSearchWord, List<char> failedChars, List<char> guessed)
    {
        IEnumerable<string> fullSearchResultList = GetFullSearchResultList(fullSearchWord, failedChars);

        Dictionary<char, int> letterFullSearchResultOccurences = Alphabet.Except(guessed).ToDictionary(x => x, x => 0);
        foreach (var possibleResultWord in fullSearchResultList)
        {
            for (int i = 0; i < possibleResultWord.Length; i++)
            {
                char currentChar = possibleResultWord[i];
                if (letterFullSearchResultOccurences.ContainsKey(currentChar))
                {
                    letterFullSearchResultOccurences[currentChar]++;
                }
            }
        }

        return letterFullSearchResultOccurences;
    }

    private IEnumerable<string> GetFullSearchResultList(string fullSearchWord, List<char> failedChars)
    {
        using SqliteConnection conn = DbConfiguration.GetDatabaseConnection();
        conn.Open();
        using SqliteCommand command = conn.CreateCommand();

        command.CommandText = $"Select * from words where word like '%{fullSearchWord}%' ";
        
        for (var index = 0; index < failedChars.Count; index++)
        {
            var c = failedChars[index];
            command.CommandText += $"AND instr(word, '{c}') > 0 ";
        }

        command.CommandText += ";";
        Console.WriteLine(command.CommandText);

        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            yield return reader.GetString(1);
        }
    }

    private int GetLastLetterIndext(string word)
    {
        if (word.Length == 0)
            return -1;

        for (int i = word.Length -1; i > 0; i--)
        {
            if (word[i] != '_')
                return i;
        }

        return -1;
    }

    private int GetFirstLetterIndex(string word)
    {
        if (word.Length == 0)
            return -1;

        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] != '_')
                return i;
        }

        return -1;
    }

    public int Complete(Result result)
    {
        AddNewWord.AddWordToDatabase(result.word!);
        BotResultTextWriter.WriteText(result.word!, BotName);
        Console.WriteLine($"Word:{result.word}, Guessed:{result.guessed.ToArray()}, result:{result.players[0].score}");
        return 0;
    }
}