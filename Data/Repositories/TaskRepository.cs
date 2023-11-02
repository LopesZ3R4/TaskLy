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

        public Tasks? GetTaskbyID(BigInteger Id)
        {
            return _context.Tasks.FirstOrDefault(t => t.Id == Id);
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
        public void Add(Tasks Task)
        {
            _context.Tasks.Add(Task);
            _context.SaveChanges();
        }

        public void Update(Tasks Task)
        {
            _context.Tasks.Update(Task);
            _context.SaveChanges();
        }
        public void FinnishTasks(List<Tasks>? Tasks)
        {
            var now = DateTime.Now;
            if(Tasks != null){
                foreach (Tasks task in Tasks)
                {
                    task.Finished = true;
                    _context.Tasks.Update(task);
                    _context.SaveChanges();
                }
            }
        }
    }
}