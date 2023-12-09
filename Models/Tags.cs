// Path: Models/Tags.cs
using System.ComponentModel.DataAnnotations;

public class Tags
{
    [Key]
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Color { get; private set; }
    public string? Owner { get; private set; }

    public Tags(int id, string name, string color, string? owner)
    {
        Id = id;
        Name = name;
        Color = color;
        Owner = owner;
    }

    public int GetId() => Id;
    public void SetId(int id) => Id = id;

    public string GetName() => Name;
    public void SetName(string name) => Name = name;

    public string GetColor() => Color;
    public void SetColor(string color) => Color = color;

    public string? GetOwner() => Owner;
    public void SetOwner(string? owner) => Owner = owner;
}