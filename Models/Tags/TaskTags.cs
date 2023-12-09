// Path: Models/Tags/TaskTags.cs
public class TaskTags
{
    public int TaskId { get; private set; }
    public int TagId { get; private set; }
    public string Owner { get; private set; }
    public TaskTags(int TaskId, int TagId, string Owner){
        this.TaskId = TaskId;
        this.TagId = TagId;
        this.Owner = Owner;
    }
    public int GetTaskId() => TaskId;
    public void SetTaskId(int taskid) => TaskId = taskid;
    public int GetTagId() => TagId;
    public void SetTagId(int tagid) => TagId = tagid;
    public string GetOwner() => Owner;
    public void SetOwner(string owner) => Owner = owner;
}