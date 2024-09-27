using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Primitives;

namespace Hackathon2024.SqlHelper;

public class PossibleWordList()
{
    private readonly List<string> _possibleWordList = [];
    private readonly StringBuilder _sb = new StringBuilder();

    public List<string> GetPossibleWordList(string word, List<char> guessed)
    {
        var sql = BuildSql(word, guessed);
        try
        {
            _sb.Clear();
            using var connection = DbConfiguration.GetDatabaseConnection();
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
    
    private string BuildSql(string word, List<char> guessed)
    {
        _sb.Append(GetSelectStatement(word));
        
        
        if (guessed.Count > 0)
        {
            for (int i = 0; i < guessed.Count; i++)
            {
                var c = guessed[i].ToString().ToUpper();
                _sb.Append(" AND not instr(word, '");
                _sb.Append(c);
                _sb.Append("') > 0");
            }
        }
        

        return _sb.ToString();
    }

    private string GetSelectStatement(string word)
    {
        return "SELECT * FROM " + DbConfiguration.GetTableName() + " WHERE "
             + DbConfiguration.GetColumnName() + " LIKE '%" + word + "%'";
    }
}