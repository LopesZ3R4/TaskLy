// .\Data\Repositories\TaskRepository.cs
using System.Numerics;
using System.Runtime.InteropServices;
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

        public async Task<Tasks?> GetTaskbyID(long Id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == Id);
        }
        public bool Exists(long Id)
        {
            return _context.Tasks.Any(t => t.Id == Id);
        }
        public async Task<List<Tasks>?> GetAllUserTasksAsync(string Username)
        {
            var query = _context.Tasks
                        .AsNoTracking();
            query = query.Where(t => t.Owner == Username);
            return await query.ToListAsync();
        }
        public async Task<List<Tasks>?> GetUserPendingTasks(string Username)
        {
            var query = _context.Tasks
                        .AsNoTracking();
            DateTime now = DateTime.Now;
            query = query.Where(t => t.Owner == Username && t.AutoFinish == true && t.Finished == false && t.FinishDate <= now);
            return await query.ToListAsync();
        }
        public async Task Add(Tasks Task)
        {
            _context.Tasks.Add(Task);
            await _context.SaveChangesAsync();
        }

        public void Update(Tasks Task)
        {
            _context.Tasks.Update(Task);
            _context.SaveChanges();
        }
        public void FinnishTasks(List<Tasks>? Tasks)
        {
            if(Tasks != null){
                foreach (Tasks task in Tasks)
                {
                    task.Finished = true;
                    _context.Tasks.Update(task);
                    _context.SaveChanges();
                }
            }
        }
        public async Task Delete(Tasks task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}