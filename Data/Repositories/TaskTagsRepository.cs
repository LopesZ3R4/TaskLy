// Path: Repositories/TaskTagsRepository.cs
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class TaskTagsRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskTagsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Exists(TaskTags taskTag)
        {
            var exists = _context.TaskTags.Any(tt => tt.Owner == taskTag.Owner && tt.TaskId == taskTag.TaskId && tt.TagId == taskTag.TagId);
            return exists;
        }
        public List<TaskTags> GetTaskTagsList(Tasks task)
        {
            var taskTags = _context.TaskTags.Where(tt => tt.Owner == task.Owner && tt.TaskId == task.Id)
                .ToList();
            return taskTags;
        }
        public async Task CreateTag(TaskTags taskTag)
        {
            _context.TaskTags.Add(taskTag);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTag(TaskTags taskTag)
        {
            _context.Entry(taskTag).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTag(TaskTags taskTag)
        {
            var tasktag = await _context.TaskTags.FindAsync(taskTag.TaskId, taskTag.TagId,taskTag.Owner);

            _context.TaskTags.Remove(tasktag!);
            await _context.SaveChangesAsync();
        }
    }
}