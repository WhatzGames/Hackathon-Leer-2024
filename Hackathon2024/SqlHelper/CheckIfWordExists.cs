using System.Data;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace Hackathon2024.SqlHelper;

public static class CheckIfWordExists
{
    public static bool WordIsInDb(string word)
    {
        var sql = MatchCountQuery(word);
        using var connection = DbConfiguration.GetDatabaseConnection();
        using var command = new SqliteCommand(sql, connection);
        command.Parameters.Add("@word", SqliteType.Text).Value = word;
        
        connection.Open();
        if (Convert.ToInt32(command.ExecuteScalar()) == 0)
        {
            connection.Close();
            return false;
        }
        connection.Close();
        return true;
    }

    private static string MatchCountQuery(string word)
    {
        return "SELECT COUNT(*) FROM " + DbConfiguration.GetTableName() + " WHERE " + DbConfiguration.GetColumnName() + " = @word";
    }
}