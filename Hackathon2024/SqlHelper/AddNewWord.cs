using Microsoft.Data.Sqlite;

namespace Hackathon2024.SqlHelper;

public static class AddNewWord
{
    public static void AddWordToDatabase(string word)
    {
        if (CheckIfWordExists.WordIsInDb(word))
        {
            return;
        }
        
        var sql = BuildSql(word);
        
        using var connection = DbConfiguration.GetDatabaseConnection();

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.Add(word);

        try
        {
            command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private static string BuildSql(string word)
    {
        return "INSERT INTO " + DbConfiguration.GetTableName() + $"({DbConfiguration.GetColumnName()}) values (@word)";
    }
}