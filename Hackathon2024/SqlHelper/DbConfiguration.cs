using Microsoft.Data.Sqlite;

namespace Hackathon2024.SqlHelper;

public class DbConfiguration
{
    private const string TableName = "words";
    private const string ColumnName = "word";
    
    public DbConfiguration()
    {
        
    }

    public static SqliteConnection GetDatabaseConnection(string dir)
    {
        //var dataSource = GetDirectory();
        var dataSource = GetCustomDirectory(dir);
        return new SqliteConnection(dataSource);
    }
    
    public static SqliteConnection GetDatabaseConnectionToSave()
    {
        var dataSource = GetDirectory();
        return new SqliteConnection(dataSource);
    }

    private static string GetCustomDirectory(string dir)
    {
        return "Data Source =" + dir;
    }

    private static string GetDirectory()
    {
        return "Data Source =" + Environment.CurrentDirectory + "\\db.sqlite";
    }

    public static string GetTableName()
    {
        return TableName;
    }

    public static string GetColumnName()
    {
        return ColumnName;
    }
}