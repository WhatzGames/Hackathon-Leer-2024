using Microsoft.Data.Sqlite;

namespace Hackathon2024.SqlHelper;

public class PossibleWordList(DbConfiguration dbConfiguration)
{
    private readonly List<string> _possibleWordList = [];

    public List<string> GetPossibleWordList(string word)
    {
        var sql = BuildSql(word);
        try
        {
            using var connection = dbConfiguration.GetDatabaseConnection();
            connection.Open();
            using var command = new SqliteCommand(sql, connection);

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _possibleWordList.Add(reader.GetString(1));
                }
            }
            else
            {
                Console.WriteLine("No entrys found in database for " + word);
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return _possibleWordList;
    }
    
    private string BuildSql(string word)
    {
        return "SELECT * FROM " + dbConfiguration.GetTableName() + " WHERE "
             + dbConfiguration.GetColumnName() + " LIKE '%" + word + "%'";
    }
}