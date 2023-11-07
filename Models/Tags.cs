// Path: Models/Tags.cs
using System.ComponentModel.DataAnnotations;

public class Tags
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Color { get; set; }
    public string? Owner { get; set; }
}