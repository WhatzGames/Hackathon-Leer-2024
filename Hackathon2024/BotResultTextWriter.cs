using Hackathon2024.SqlHelper;

namespace Hackathon2024;

public static class BotResultTextWriter
{
    public static void WriteText(string text, string botName)
    {
        string path = $"{botName}.txt";
        
        if (CheckIfWordExists(path, text))
        {
            return;
        }
        
        StreamWriter writer = new StreamWriter(path);
        writer.WriteLine(text);
        writer.Close();
    }

    private static bool CheckIfWordExists(string filename, string text)
    {
        StreamReader reader = new StreamReader(filename, new FileStreamOptions{Mode = FileMode.OpenOrCreate, Share = FileShare.ReadWrite}); 
        while (!reader.EndOfStream)
        {
            if (reader.ReadLine()!.Equals(text))
            {
                return true;
            }
        }
        
        return false;
    }
}