namespace Hackathon2024.DTO;

public class Init : Game
{
    public string? id { get; set; }
    public List<Player>? players { get; set; }
    public List<LogEntry>? log { get; set; }
    public string? self { get; set; }
}