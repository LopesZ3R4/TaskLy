// Path: Models/Tags/TaskTags.cs
public class TaskTags
{
    public int TaskId { get; set; }
    public Tasks Tasks { get; set; }
    public int TagId { get; set; }
    public Tags Tags { get; set; }
    public string Owner { get; set; }
}