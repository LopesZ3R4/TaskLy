// Path: Models/Tags/TaskTags.cs
public class TaskTags
{
    public int TaskId { get; set; }

    public int TagId { get; set; }
    public Tags Tag { get; set; }
    public string Username { get; set; }
}