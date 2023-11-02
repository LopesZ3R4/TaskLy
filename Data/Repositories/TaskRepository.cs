// .\Data\Repositories\TaskRepository.cs
using System.Numerics;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class TaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task? GetTaskbyID(BigInteger Id)
        {
            return _context.Tasks.FirstOrDefault(t => t.Id == Id);
        }
        public async Task<List<Task>?> GetAllUserTasksAsync(string Username)
        {
            var query = _context.Tasks
                        .AsNoTracking();
            query = query.Where(t => t.Owner == Username);
            return await query.ToListAsync();
        }
        public void Add(Task Task)
        {
            _context.Tasks.Add(Task);
            _context.SaveChanges();
        }

        public void Update(Task Task)
        {
            _context.Tasks.Update(Task);
            _context.SaveChanges();
        }
    }
}