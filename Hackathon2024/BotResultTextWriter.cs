namespace Hackathon2024;

public static class BotResultTextWriter
{
    public static void WriteText(string text, string botName)
    {
        StreamWriter writer = new StreamWriter($"{botName}.txt");
        writer.WriteLine(text);
        writer.Close();
    }
}