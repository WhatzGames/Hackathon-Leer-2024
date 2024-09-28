
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
            string sanitizedLine = SanitizeLine(currentSplitLine).ToUpper();
            
            if (!WordHasValidChars(sanitizedLine))
            {
                continue;
            }
            
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
        .Replace("Ö", "oe")
        .Replace("ß", "ss")
        .Replace("Ý", "y")
        .Replace("ý", "y")
        .Replace("Û", "u")
        .Replace("Ú", "u")
        .Replace("Ù", "u")
        .Replace("û", "u")
        .Replace("ù", "u")
        .Replace("ú", "u")
        .Replace("Õ", "o")
        .Replace("Ô", "o")
        .Replace("Ó", "o")
        .Replace("Ò", "o")
        .Replace("ó", "o")
        .Replace("ô", "o")
        .Replace("õ", "o")
        .Replace("ó", "o")
        .Replace("Ñ", "n")
        .Replace("ñ", "n")
        .Replace("Ð", "d")
        .Replace("Ï", "i")
        .Replace("Î", "i")
        .Replace("Í", "i")
        .Replace("Ì", "i")
        .Replace("ì", "i")
        .Replace("í", "i")
        .Replace("î", "i")
        .Replace("ï", "i")
        .Replace("Ë", "e")
        .Replace("Ê", "e")
        .Replace("É", "e")
        .Replace("È", "e")
        .Replace("è", "e")
        .Replace("é", "e")
        .Replace("ê", "e")
        .Replace("ë", "e")
        .Replace("à", "a")
        .Replace("á", "a")
        .Replace("â", "a")
        .Replace("ã", "a")
        .Replace("å", "a")
        .Replace("æ", "ae")
        .Replace("Æ", "ae")
        .Replace("Å", "a")
        .Replace("Â", "a")
        .Replace("Á", "a")
        .Replace("À", "a");
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