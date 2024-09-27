using Microsoft.Data.Sqlite;

string connstring = @"Data Source =C:\Dev\Hackathon-Leer-2024\db.sqlite";
Console.WriteLine("Start creating raw dictionary");

Dictionary<string, ulong> letterPairCount = new ();
for (char c = 'A'; c <= 'Z'; c++)
{
    for (char h = 'A'; h <= 'Z'; h++)
    {
        letterPairCount[$"{c}{h}"] = 0;
        for (char a = 'A'; a <= 'Z'; a++)
        {
            letterPairCount[$"{c}{h}{a}"] = 0;
        }
    }
}
using SqliteConnection conn = new SqliteConnection(connstring);
conn.Open();
    
using var command = conn.CreateCommand();

command.CommandText = "Select word from words";

var reader = command.ExecuteReader();

while (reader.Read())
{
    string currentWord = reader.GetString(0);

    for (int i = 0; i < currentWord.Length - 2; i++)
    {
        string letterpair = currentWord.Substring(i, 2).ToUpper();
        if (WordHasValidChars(letterpair))
            letterPairCount[letterpair]++;
    }
    for (int i = 0; i < currentWord.Length - 3; i++)
    {
        string letterpair = currentWord.Substring(i, 3).ToUpper();
        if (WordHasValidChars(letterpair))
            letterPairCount[letterpair]++;
    }
}

foreach (var kv in letterPairCount.OrderBy(x =>x.Value))
{
    Console.WriteLine($"{kv.Key}; {kv.Value}");
}

bool WordHasValidChars(string word)
{
    return word.All(c => c >= 65 && c <= 90 || c >= 97 && c <= 122);
}
CreateTableIfNotExists();

FillTableWithLetterPairs(letterPairCount);

void FillTableWithLetterPairs(Dictionary<string, ulong> dictionary)
{
    using SqliteConnection conn = new SqliteConnection(connstring);
    conn.Open();
    
    foreach (var keyValuePair in dictionary)
    {
        Console.WriteLine(keyValuePair.Key + "; " + keyValuePair.Value);
        using var command1= conn.CreateCommand();
        command1.CommandText = "Select length from letter_indices " +
                              $"where letter_pair = '{keyValuePair.Key}' ";
        var result = Convert.ToInt64(command1.ExecuteScalar() ?? 0);

        if (result == 0)
        {
            using var command2 = conn.CreateCommand();
            command2.CommandText = "Insert into letter_indices " +
                                  $"(letter_pair,length,occurences) VALUES " +
                                  $"('{keyValuePair.Key}',{keyValuePair.Key.Length},{keyValuePair.Value})";
            command2.ExecuteNonQuery();
        }
        else
        {
            using var command3 = conn.CreateCommand();
            command3.CommandText = "Update letter_indices " +
                                   $"set occurences = {keyValuePair.Value} " +
                                  $"where letter_pair = '{keyValuePair.Key}' ";
            command3.ExecuteNonQuery();
        }
        
    }
}

void CreateTableIfNotExists()
{
    using SqliteConnection conn = new SqliteConnection(connstring);
    conn.Open();
    
    using var command = conn.CreateCommand();

    command.CommandText = "create table IF NOT EXISTS letter_indices " +
                          "(letter_pair text PRIMARY KEY, " +
                          "length INTEGER NOT NULL, " +
                          "occurences INTEGER NOT NULL)";
    command.ExecuteNonQuery();
}
