﻿namespace Hackathon_24.DTO;

public class Result
{
    public string? id { get; set; }
    public List<Player>? players { get; set; }
    public string? word { get; set; }
    public List<char>? guessed { get; set; }
    public List<LogEntry>? log { get; set; }
    public string? result { get; set; }
    public string? self { get; set; }
}