namespace Hackathon2024.DTO ;

    public class Init : Game
    {
        public Guid id { get; set; }
        public List<Player>? players { get; set; }
        public List<LogEntry>? log { get; set; }
        public Guid self { get; set; }
    }