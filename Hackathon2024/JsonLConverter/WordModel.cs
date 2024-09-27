using System.Text.Json.Serialization;

namespace Hackathon_24.JsonLConverter;

public class WordModel
{
    [JsonPropertyName("word")]
    public string word { get; set; } = String.Empty;
}