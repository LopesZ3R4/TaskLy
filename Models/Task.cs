//File Path: /Models/Task.cs
using System.Numerics;

public class Task
{
    public int Id { get; set; }
    #pragma warning disable CS8618
    public string Title { get; set; }
    public DateTime StartTime { get; set; }
    public BigInteger Duration { get; set; }
    public bool AutoFinish { get; set; }
    public bool Finished { get; set; }
    public string? Owner { get; set; }
}