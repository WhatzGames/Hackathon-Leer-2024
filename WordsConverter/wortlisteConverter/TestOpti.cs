using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Microsoft.Data.Sqlite;
namespace wortlisteConverter;

public class TestOpti
{
    private string dataDir = Path.Combine(Environment.CurrentDirectory, "data");
    private string pathToSqlite = @"Data Source=C:\Users\Kurt\RiderProjects\Hackathon 2024\Hackathon-Leer-2024\db.sqlite";
    private List<string> _list = new List<string>();
    
    
    public void DoFaster()
    {
        string dataDir = Path.Combine(Environment.CurrentDirectory, "data");
        
        foreach (string file in Directory.EnumerateFiles(dataDir))
        {
            int counter = 0;
            var originalLines = File.ReadLines(file);

            foreach (string line in originalLines)
            {
                if (!WordHasValidChars(line))
                {
                    continue;
                }

                string sanitizedLine = SanitizeLine(line).ToUpper();

                if (sanitizedLine.Length >= 5)
                {
                    if (!WordExists(sanitizedLine))
                    {
                        _list.Add(sanitizedLine);
                        counter++;
                        InsertWord(sanitizedLine);
                    }
                }

                if (counter == 100000)
                {
                    InsertWordList();
                    Console.WriteLine("added 100k entries");
                    _list.Clear();
                }
            }
        }

    }
    private string SanitizeLine(string s)
    {
        return s.Replace("ä", "ae")
                .Replace("ö", "oe")
                .Replace("ü", "ue")
                .Replace("Ä", "ae")
                .Replace("Ü", "ue")
                .Replace("Ö", "öe")
                .Replace("ß", "ss");
    }

    private bool WordHasValidChars(string word)
    {
        return word.All(c => c >= 65 && c <= 90 || c >= 97 && c <= 122 || c == 196 || c == 214 || c == 220 || c == 228 || c == 246 || c == 252 || c == 223);
    }

    private bool WordExists(string word)
    {
        using var conn = new SqliteConnection(pathToSqlite);
        conn.Open();
        using var command = conn.CreateCommand();

        command.CommandText = "Select Count(*) from words where word = @word";
        command.Parameters.Add(new SqliteParameter() { ParameterName = "@word", Value = word });

        var result = (long)(command.ExecuteScalar() ?? 0);
        return result != 0;
    }

    private bool InsertWord(string word)
    {
        using var conn = new SqliteConnection(pathToSqlite);
        conn.Open();
        using var command = conn.CreateCommand();

        command.CommandText = "Insert into words (word) VALUES (@word)";
        command.Parameters.Add(new SqliteParameter() { ParameterName = "@word", Value = word });
        return command.ExecuteNonQuery() > 0;
    }

    private bool InsertWordList()
    {
        using var conn = new SqliteConnection(pathToSqlite);
        conn.Open();
        using var command = conn.CreateCommand();

        command.CommandText = GetCommandText();
        for (int i = 0; i < _list.Count; i++)
        {
            command.Parameters.Add(new SqliteParameter()
            {
                ParameterName = $"@word{i}",
                Value = _list[i]
            });
        }
        return command.ExecuteNonQuery() > 0;
    }

    private string GetCommandText()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _list.Count; i++)
        {
            sb.Append("Insert into words (word) VALUES (@word");
            if (i == _list.Count)
            {
                sb.Append(i);
                sb.Append(")");
            }

            sb.Append(i);
            sb.Append(";");
        }

        return sb.ToString();
    }
}