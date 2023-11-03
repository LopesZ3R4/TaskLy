// Path: Models/Holiday.cs
using System.ComponentModel.DataAnnotations.Schema;
public class Holiday
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string LocalName { get; set; }
    public string Name { get; set; }
    public string CountryCode { get; set; }
    public string? CountyCode { get; set; }

    [ForeignKey("CountyCode")]
    public County? County { get; set; }
    public bool Fixed { get; set; }
    public bool Global { get; set; }
    public int? LaunchYear { get; set; }
    public List<HolidayType> HolidayTypes { get; set; }
}