
using Microsoft.Data.Sqlite;

string dataDir = Path.Combine(Environment.CurrentDirectory, "data");
string pathToSqlite = @"Data Source=C:\Dev\Hackathon-Leer-2024\db.sqlite";

Console.WriteLine("Hello, World!");

foreach (string file in Directory.EnumerateFiles(dataDir))
{
    var originalLines = File.ReadLines(file);
    Console.WriteLine($"Start reading {file}");

    int count = 0;
    
    foreach (string line in originalLines)
    {
        try
        {
            var currentSplitLine = line.Split(' ')[0];
            if (!WordHasValidChars(currentSplitLine))
            {
                continue;
            }

            string sanitizedLine = SanitizeLine(currentSplitLine).ToUpper();

            if (sanitizedLine.Length >= 5)
            {
                if (!WordExists(sanitizedLine))
                {
                    InsertWord(sanitizedLine);
                    count++;
                }
            }

            if (count % 100_000 == 0 && count > 0)
            {
                Console.WriteLine($"Added {count} entries already");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine($"Could not work with line {line}");
        }
    }
    Console.WriteLine($"Added {count} entries");
}


string SanitizeLine(string s)
{
    return s.Replace("ä", "ae")
        .Replace("ö", "oe")
        .Replace("ü", "ue")
        .Replace("Ä", "ae")
        .Replace("Ü", "ue")
        .Replace("Ö", "öe")
        .Replace("ß", "ss");
}

bool WordHasValidChars(string word)
{
    return word.All(c => c >= 65 && c <= 90 || c >= 97 && c <= 122 || c == 196 || c == 214 || c == 220 || c == 228 || c == 246 || c == 252 || c == 223);
}

bool WordExists(string word)
{
    using var conn = new SqliteConnection(pathToSqlite);
    conn.Open();
    using var command = conn.CreateCommand();

    command.CommandText = "Select Count(*) from words where word = @word";
    command.Parameters.Add(new SqliteParameter() { ParameterName = "@word", Value = word });

    var result = (long)(command.ExecuteScalar() ?? 0);
    return result != 0;
}

bool InsertWord(string word)
{
    using var conn = new SqliteConnection(pathToSqlite);
    conn.Open();
    using var command = conn.CreateCommand();

    command.CommandText = "Insert into words (word) VALUES (@word)";
    command.Parameters.Add(new SqliteParameter() { ParameterName = "@word", Value = word });
    return command.ExecuteNonQuery() > 0;
}