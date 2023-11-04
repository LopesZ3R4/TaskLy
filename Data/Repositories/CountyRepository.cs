// File: c:\TaskLy\Data\Repositories\CountyRepository.cs
using Microsoft.EntityFrameworkCore;

public class CountyRepository
{
    private readonly ApplicationDbContext _context;

    public CountyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public County? GetCounty(string countyCode)
    {
        return _context.County.FirstOrDefault(c => c.CountyCode == countyCode);
    }
    public bool Exists(string countyCode)
    {
        return _context.County.Any(c=> c.CountyCode == countyCode);
    }

    public async Task<IEnumerable<County>> GetAllCounties()
    {
        return await _context.County.ToListAsync();
    }

    public async Task AddCounty(County county)
    {
        await _context.County.AddAsync(county);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCounty(County county)
    {
        _context.County.Update(county);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCounty(string countyCode)
    {
        var county = await _context.County.FindAsync(countyCode);
        if (county != null)
        {
            _context.County.Remove(county);
            await _context.SaveChangesAsync();
        }
    }
}