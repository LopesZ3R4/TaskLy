// Path: Models/Tags.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Tags
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string color { get; set; }
    public string? Username { get; set; }
    
}