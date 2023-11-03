// Path: Repositories/TagRepository.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class TagsRepository : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TagsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetNextId(string username)
        {
            var maxId = await _context.Tags
                .Where(t => t.Username == username)
                .MaxAsync(t => (int?)t.Id);

            return (maxId ?? 0) + 1;
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