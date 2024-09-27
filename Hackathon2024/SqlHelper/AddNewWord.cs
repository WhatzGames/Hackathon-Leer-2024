using System.Data;
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
        command.Parameters.Add("@word", SqliteType.Text).Value = word;

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            connection.Close();
        }
    }

    private static string BuildSql(string word)
    {
        return "INSERT INTO " + DbConfiguration.GetTableName() + $"({DbConfiguration.GetColumnName()}) values (@word)";
    }
}