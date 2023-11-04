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
    public List<TagDto> Tag { get; set; }
}

public class TagDto
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
}