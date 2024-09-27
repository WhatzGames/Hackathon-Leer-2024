using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Security.AccessControl;

namespace Hackathon_24.DTO;

public class Round
{
    public string? id { get; set; }
    public List<Player>? players { get; set; }
    public string? word { get; set; }
    public List<char>? guessed { get; set; }
    public List<LogEntry>? log { get; set; }
    public string? type { get; set; }
    public string? self { get; set; }
}