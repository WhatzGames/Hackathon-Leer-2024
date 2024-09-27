using Microsoft.Data.Sqlite;

namespace Hackathon2024.SqlHelper;

public class DbConfiguration
{
    private readonly string _rootDirectory = Environment.CurrentDirectory;
    private const string TableName = "words";
    private const string ColumnName = "word";
    
    public DbConfiguration()
    {
        
    }

    public SqliteConnection GetDatabaseConnection()
    {
        var dataSource = GetDirectory();
        return new SqliteConnection(dataSource);
    }

    private string GetDirectory()
    {
        return "Data Source =" + _rootDirectory + "\\db.sqlite";
    }

    public string GetTableName()
    {
        return TableName;
    }

    public string GetColumnName()
    {
        return ColumnName;
    }
}