// Path: c:\TaskLy\Services\HolidayService.cs
using System.Runtime.InteropServices;
using Data;
using Newtonsoft.Json;

public class HolidayService
{
    private readonly HolidayRepository _holidayRepository;

    public HolidayService(HolidayRepository holidayRepository)
    {
        _holidayRepository = holidayRepository;
    }
    public async Task ExecuteAsync()
    {
        using (var client = new HttpClient())
        {
            int year = DateTime.Today.Year;
            string countrycode = "BR";
            var tokenEndpoint = new Uri($"https://date.nager.at/api/v3/publicholidays/{year}/{countrycode}");

            var response = await client.GetAsync(tokenEndpoint);
            var responseContent = await response.Content.ReadAsStringAsync();

            var holidayData = JsonConvert.DeserializeObject<List<HolidayData>>(responseContent);
            var newHolidays = new List<Holiday>();

            foreach (var apiHoliday in holidayData)
            {
                if (apiHoliday.Global)
                {
                    var newHoliday = new Holiday
                    {
                        Date = apiHoliday.Date,
                        LocalName = apiHoliday.LocalName,
                        Name = apiHoliday.Name,
                        CountryCode = apiHoliday.CountryCode,
                        Fixed = apiHoliday.Fixed,
                        Global = apiHoliday.Global,
                        County = null,
                        LaunchYear = apiHoliday.LaunchYear,
                        HolidayTypes = ConvertTypesToHolidayTypes(apiHoliday.Types)
                    };
                    bool exists = _holidayRepository.Exists(newHoliday);
                    if(!exists)
                    {
                        _holidayRepository.Add(newHoliday);
                    }
                }
                else if (apiHoliday.Counties != null)
                {
                    foreach (var county in apiHoliday.Counties)
                    {
                        var newHoliday = new Holiday
                        {
                            Date = apiHoliday.Date,
                            LocalName = apiHoliday.LocalName,
                            Name = apiHoliday.Name,
                            CountryCode = apiHoliday.CountryCode,
                            Fixed = apiHoliday.Fixed,
                            Global = apiHoliday.Global,
                            CountyCode = county,
                            LaunchYear = apiHoliday.LaunchYear,
                            HolidayTypes = ConvertTypesToHolidayTypes(apiHoliday.Types)
                        };
                        bool exists = _holidayRepository.Exists(newHoliday);
                        if(!exists)
                        {
                            _holidayRepository.Add(newHoliday);
                        }
                    }
                }
            }
        }
    }

    private List<HolidayType> ConvertTypesToHolidayTypes(List<string> types)
    {
        var holidayTypes = new List<HolidayType>();

        foreach (var type in types)
        {
            var holidayType = new HolidayType
            {
                Type = new Type { TypeName = type }
            };

            holidayTypes.Add(holidayType);
        }

        return holidayTypes;
    }
}