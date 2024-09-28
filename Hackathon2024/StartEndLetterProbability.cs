using Hackathon2024.SqlHelper;
using Microsoft.Data.Sqlite;

namespace Hackathon2024;

public static class StartEndLetterProbability
{
    /*public static (Dictionary<char, double>, Dictionary<char, double>) GetLetterStartEndProbability()
    {
        List<char> letters = ['A', 'B', 'C', 'D', 'E', 
            'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
        'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
        
        Dictionary<char, double> startLetterProbability = new Dictionary<char, double>();
        Dictionary<char, double> endLetterProbability = new Dictionary<char, double>();
        
        var allCases = BuildSqlForAllCases();
        using var connection = DbConfiguration.GetDatabaseConnection();
        using var allCasesCommand = new SqliteCommand(allCases, connection);
        connection.Open();
        foreach (char letter in letters)
        {
            var start = BuildSqlForSingleStartLetterCases(letter);
            var end = BuildSqlForSingleEndLetterCases(letter);
            
            double allLetters = Convert.ToDouble(allCasesCommand.ExecuteScalar());
            
            using var startCommand = new SqliteCommand(start, connection);
            
            using var endCommand = new SqliteCommand(end, connection);
            
            var startCount = Convert.ToDouble(startCommand.ExecuteScalar());
            var probabilityStart = Math.Round(100 / (allLetters / startCount), 2);
            startLetterProbability.Add(letter, probabilityStart);
            
            var endCount = Convert.ToDouble(endCommand.ExecuteScalar());
            var probabilityEnd = Math.Round(100 / (allLetters / endCount), 2);
            endLetterProbability.Add(letter, probabilityEnd);
        }
        connection.Close();
        
        return (startLetterProbability, endLetterProbability);
    }*/
    
    private static string BuildSqlForAllCases()
    {
        return "SELECT COUNT(*) FROM " + DbConfiguration.GetTableName();
    }

    private static string BuildSqlForSingleStartLetterCases(char letter)
    {
        return "SELECT COUNT(*) FROM " + DbConfiguration.GetTableName() + " WHERE " + DbConfiguration.GetColumnName() + " LIKE '" + $"{letter}" + "%'";
    }
    
    private static string BuildSqlForSingleEndLetterCases(char letter)
    {
        return "SELECT COUNT(*) FROM " + DbConfiguration.GetTableName() + " WHERE " + DbConfiguration.GetColumnName() + " LIKE '%" + $"{letter}" + "'";
    }
}