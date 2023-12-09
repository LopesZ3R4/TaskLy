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
    public int Id { get; private set; }
    #pragma warning disable CS8618
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public DateTime StartDate { get; private set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime FinishDate { get; private set; }
    public long Duration { get; private set; }
    public bool AutoFinish { get; private set; }
    public TaskStatus Status { get; private set; }
    public string? Owner { get; private set; }
    public Tasks(int Id, string Title, string? Description, DateTime StartDate, DateTime FinishDate, long Duration, bool AutoFinish, TaskStatus Status, string? Owner)
    {
        this.Id = Id;
        this.Title = Title;
        this.Description = Description;
        this.StartDate = StartDate;
        this.Duration = Duration;
        this.AutoFinish = AutoFinish;
        this.Status = Status;
        this.Owner = Owner;
    }

    public int GetId() => Id;
    public void SetId(int id) => Id = id;

    public string GetTitle() => Title;
    public void SetTitle(string title) => Title = title;

    public string? GetDescription() => Description;
    public void SetDescription(string? description) => Description = description;

    public DateTime GetStartDate() => StartDate;
    public void SetStartDate(DateTime startDate) => StartDate = startDate;

    public DateTime GetFinishDate() => FinishDate;
    public void SetFinishDate(DateTime finishDate) => FinishDate = finishDate;

    public long GetDuration() => Duration;
    public void SetDuration(long duration) => Duration = duration;

    public bool isAutoFinish() => AutoFinish;
    public void SetAutoFinish(bool autoFinish) => AutoFinish = autoFinish;

    public TaskStatus GetStatus() => Status;
    public void SetStatus(TaskStatus status) => Status = status;

    public string? GetOwner() => Owner;
    public void SetOwner(string? owner) => Owner = owner;
}