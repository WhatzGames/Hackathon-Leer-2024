using System.Text;
using System.Text.Json;

namespace Hackathon_24.JsonLConverter;

public class JSONLToTxt
{
    public void GetFile()
    {
        List<string> words = Words();
        
        StreamWriter writer = new StreamWriter("kaikki.txt");
        foreach (string word in words)
        { 
            writer.WriteLine(word);    
        }
        writer.Close();
    }

    private List<string> Words()
    {
        var file = "kaikki.org-dictionary-German-words.jsonl";

        var words = new List<string>();
        
        using var stream = new StreamReader(file, Encoding.UTF8);

        while (!stream.EndOfStream)
        {
            var jsonLine = stream.ReadLine();

            if (jsonLine != null)
            {
                var word = JsonSerializer.Deserialize<WordModel>(jsonLine);
                if (word != null)
                    words.Add(word.word);
            }
        }

        return words;
    }
}
