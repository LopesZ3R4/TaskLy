// File Path: .\Models\TaskDto.cs
public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime FinishDate { get; set; }
    public long Duration { get; set; }
    public bool AutoFinish { get; set; }
    public TaskStatus Status { get; set; }
    public string? Owner { get; set; }
    public List<TagDto> Tags { get; set; }
    public Tasks ToModel()
    {
        return new Tasks
        {
            Id = this.Id,
            Title = this.Title,
            Description = this.Description,
            StartDate = this.StartDate,
            FinishDate = this.FinishDate,
            Duration = this.Duration,
            AutoFinish = this.AutoFinish,
            Status = this.Status,
            Owner = this.Owner,
        };
    }
}

public class TagDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
}