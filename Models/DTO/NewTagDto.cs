// File Path: .\Models\NewTagDto.cs
public class NewTagDto
{
    public string Name { get; set; }
    public string Color { get; set; }
    public Tags ToModel(int id, string owner)
    {
        return new Tags(
            id,
            Name,
            Color,
            owner
        );
    }
}