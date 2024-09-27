using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Security.AccessControl;

namespace Hackathon2024.DTO;

public class Round : Game
{
    public string? id { get; set; }
    public List<Player>? players { get; set; }
    public string? word { get; set; }
    public List<char>? guessed { get; set; }
    public List<LogEntry>? log { get; set; }
    public string? self { get; set; }
}