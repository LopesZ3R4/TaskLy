// Path: Repositories/TagRepository.cs
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class TagsRepository
    {
        private readonly ApplicationDbContext _context;

        public TagsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetNextId(string owner)
        {
            var maxId = await _context.Tags
                .Where(t => t.Owner == owner)
                .MaxAsync(t => (int?)t.Id);

            return (maxId ?? 0) + 1;
        }
        public bool Exists(int id, string owner)
        {
            var exists = _context.Tags.Any(t => t.Id == id && t.Owner == owner);
            return exists;
        }
        public List<dynamic> GetTagsList(string owner)
        {
            var tags = _context.Tags.Where(t => t.Owner == owner)
                .Select(t => new { t.Id, t.Name, t.Color } as dynamic)
                .ToList();
            return tags;
        }
        public async Task CreateTag(Tags tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTag(Tags tag)
        {
            _context.Entry(tag).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTag(Tags tags)
        {
            var tag = await _context.Tags.FindAsync(tags);
            if (tag == null)
            {
                throw new Exception("Tag not found");
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}