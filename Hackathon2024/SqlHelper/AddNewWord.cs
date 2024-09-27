using Microsoft.Data.Sqlite;

namespace Hackathon2024.SqlHelper;

public class AddNewWord
{
    private readonly DbConfiguration _dbConfiguration;

    public AddNewWord(DbConfiguration dbConfiguration)
    {
        _dbConfiguration = dbConfiguration;
    }

    public void AbbWordToDatabase(string word)
    {
        var sql = BuilSql(word);
        
        using var connection = _dbConfiguration.GetDatabaseConnection();

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

    private string BuilSql(string word)
    {
        return "INSERT INTO " + _dbConfiguration.GetTableName() + $"({_dbConfiguration.GetColumnName()}) values (@word)";
    }
}