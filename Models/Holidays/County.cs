// Path: Models/Holidays/County.cs
using System.ComponentModel.DataAnnotations;
public class County
{
    [Key]
    public string CountyCode  { get; set; }
    public string Name { get; set; }

}