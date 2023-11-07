//File Path: /Models/Task.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum TaskStatus
{
    Pending = 1,
    InProgress = 2,
    Finished = 3
}
public class Tasks
{
    [Key]
    public int Id { get; set; }
    #pragma warning disable CS8618
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime FinishDate { get; set; }
    public long Duration { get; set; }
    public bool AutoFinish { get; set; }
    public TaskStatus Status { get; set; }
    public string? Owner { get; set; }
}