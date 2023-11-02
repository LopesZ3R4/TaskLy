//File Path: /Models/Task.cs
using System.ComponentModel.DataAnnotations.Schema;

public class Tasks
{
    public int Id { get; set; }
    #pragma warning disable CS8618
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime FinishDate { get; set; }
    public long Duration { get; set; }
    public bool AutoFinish { get; set; }
    public bool Finished { get; set; }
    public string? Owner { get; set; }
}