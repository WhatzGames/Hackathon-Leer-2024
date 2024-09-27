using Microsoft.Data.Sqlite;

namespace Hackathon2024.SqlHelper;

public class DbConfiguration
{
    private readonly string _rootDirectory = Environment.CurrentDirectory;
    private const string TableName = "";
    private const string ColumnName = "";
    
    public DbConfiguration()
    {
        
    }

    public SqliteConnection GetDatabaseConnection()
    {
        return new SqliteConnection(GetDirectory());
    }

    private string GetDirectory()
    {
        return "@Data Source =" + _rootDirectory + "\\db.sqlite";
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