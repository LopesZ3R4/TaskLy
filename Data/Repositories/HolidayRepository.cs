// Path: Data/Repositories/HolidayRepository.cs
using Microsoft.EntityFrameworkCore;

namespace Data
    {
    public class HolidayRepository
    {
        private readonly ApplicationDbContext _context;

        public HolidayRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Holiday> GetAll()
        {
            return _context.Holidays
                .Include(h => h.HolidayTypes)
                .ThenInclude(ht => ht.Type)
                .Include(hc => hc.County)
                .ToList();
        }
        public bool Exists(Holiday Holiday)
        {
            return _context.Holidays.Any(h => h.Date == Holiday.Date && h.Name == Holiday.Name && h.CountryCode == Holiday.CountryCode && h.CountyCode == Holiday.CountyCode);
        }
        public Holiday? GetById(int id)
        {
            return _context.Holidays
                .Include(h => h.HolidayTypes)
                .ThenInclude(ht => ht.Type)
                .Include(hc => hc.County)
                .FirstOrDefault(h => h.Id == id);
        }
        public void Add(Holiday holiday)
        {
            _context.Holidays.Add(holiday);
            _context.SaveChanges();
        }

        public void Update(Holiday holiday)
        {
            _context.Holidays.Update(holiday);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var holiday = _context.Holidays.Find(id);
            if (holiday != null)
            {
                _context.Holidays.Remove(holiday);
                _context.SaveChanges();
            }
        }
    }
}