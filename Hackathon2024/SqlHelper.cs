using System.IO;
using System;
using Microsoft.Data.Sqlite;

namespace Hackathon2024;

public class SqlHelper
{
    private readonly string _word;
    private const string TableName = "";
    private const string ColumnName = "";
    private const string Directory = "";
    private readonly string _rootDirectory;
    private List<String> _possibleWordList = new List<string>();
    
    public SqlHelper(string word)
    {
        _word = word;
        _rootDirectory = Environment.CurrentDirectory;
    }

    private string BuildSql()
    {
        return "SELECT * FROM " + TableName + " WHERE "
            + ColumnName + "LIKE \'" + _word + "\'";
    }

    public List<string> GetPossibleWordList()
    {
        var sql = BuildSql();
        try
        {
            using var connection = new SqliteConnection("@Data Source =" + Directory + "\\db.sqlite");

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
                Console.WriteLine("No entrys found in database for " + _word);
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
}