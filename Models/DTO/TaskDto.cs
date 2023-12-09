// File Path: .\Models\TaskDto.cs
public class TaskDto
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime FinishDate { get; private set; }
    public long Duration { get; private set; }
    public bool AutoFinish { get; private set; }
    public TaskStatus Status { get; private set; }
    public string? Owner { get; private set; }
    public List<TagDto>? Tags { get; set; }
    public TaskDto(int Id, string Title, string? Description, DateTime StartDate, DateTime FinishDate, long Duration, bool AutoFinish, TaskStatus Status, string? Owner)
    {
        this.Id = Id;
        this.Title = Title;
        this.Description = Description;
        this.StartDate = StartDate;
        this.FinishDate = FinishDate;
        this.Duration = Duration;
        this.AutoFinish = AutoFinish;
        this.Status = Status;
        this.Owner = Owner;
    }
    public Tasks ToModel()
    {
        return new Tasks(
            Id,
            Title,
            Description,
            StartDate,
            FinishDate,
            Duration,
            AutoFinish,
            Status,
            Owner
        );
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

    public bool GetAutoFinish() => AutoFinish;
    public void SetAutoFinish(bool autoFinish) => AutoFinish = autoFinish;

    public TaskStatus GetStatus() => Status;
    public void SetStatus(TaskStatus status) => Status = status;

    public string? GetOwner() => Owner;
    public void SetOwner(string? owner) => Owner = owner;

    public List<TagDto> GetTags() => Tags ?? new List<TagDto>();
    public void SetTags(List<TagDto> tags) => Tags = tags;
}

public class TagDto
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Color { get; private set; }
    public TagDto(int Id, string Name, string Color)
    {
        this.Id = Id;
        this.Name = Name;
        this.Color = Color;
    }
    public int GetId() => Id;
    public void SetId(int id) => Id = id;
    public string GetName() => Name;
    public void SetName(string name) => Name = name;
    public string GetColor() => Color;
    public void SetColor(string color) => Color = color;
}