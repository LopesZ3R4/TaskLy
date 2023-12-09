// .\Data\Repositories\TaskRepository.cs
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class TaskRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly TaskTagsRepository _taskTagsRepository;

        public TaskRepository(ApplicationDbContext context, TaskTagsRepository taskTagsRepository)
        {
            _context = context;
            _taskTagsRepository = taskTagsRepository;
        }

        public async Task<Tasks?> GetTaskbyID(int Id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == Id);
        }
        public async Task<string?> GetTaskOwnerByID(int Id)
        {
            return await _context.Tasks
                .Where(t => t.Id == Id)
                .Select(t => t.Owner)
                .FirstOrDefaultAsync();
        }
        public bool Exists(long Id)
        {
            return _context.Tasks.Any(t => t.Id == Id);
        }
        public async Task<List<TaskDto>?> GetAllUserTasks(string Username)
        {
            var tasks = await _context.Tasks
                .Where(t => t.Owner == Username)
                .Select(t => new TaskDto(
                    t.Id,
                    t.Title,
                    t.Description,
                    t.StartDate,
                    t.FinishDate,
                    t.Duration,
                    t.AutoFinish,
                    t.Status,
                    t.Owner
                )).ToListAsync();

            foreach (var task in tasks)
            {
                var tags = await (from tt in _context.TaskTags
                                  join tag in _context.Tags on tt.TagId equals tag.Id
                                  where tt.TaskId == task.Id
                                  select new TagDto(
                                      (int)tag.GetId()!,
                                      tag.GetName(),
                                      tag.GetColor()
                                  )).ToListAsync();

                task.SetTags(tags);
            }
            return tasks;
        }
        public async Task<List<Tasks>?> GetUserPendingTasks(string Username)
        {
            var query = _context.Tasks.AsQueryable();
            DateTime now = DateTime.Now;
            query = query.Where(t => t.Owner == Username && t.AutoFinish == true && t.Status != TaskStatus.Finished && t.FinishDate <= now);
            return await query.ToListAsync();
        }
        public async Task<Task<int>> Add(TaskDto taskDto)
        {
            Tasks Task = taskDto.ToModel();
            _context.Tasks.Add(Task);

            await _context.SaveChangesAsync();

            foreach (var tagDto in taskDto.GetTags())
            {
                var taskTag = new TaskTags(
                    Task.GetId(),
                    tagDto.GetId(),
                    Task.GetOwner()!
                );

                _context.TaskTags.Add(taskTag);
            }
            return _context.SaveChangesAsync();
        }

        public async Task Update(Tasks Task)
        {
            _context.Tasks.Update(Task);
            await _context.SaveChangesAsync();
        }
        public void FinnishTasks(List<Tasks>? Tasks)
        {
            if (Tasks != null)
            {
                foreach (Tasks task in Tasks)
                {
                    task.SetStatus(TaskStatus.Finished);
                    _context.Tasks.Update(task);
                    _context.SaveChanges();
                }
            }
        }
        public async Task Delete(Tasks task)
        {
            List<TaskTags> taskTags = _taskTagsRepository.GetTaskTagsList(task);
            foreach (TaskTags taskTag in taskTags)
            {
                taskTag.SetOwner(task.GetOwner()!);
                await _taskTagsRepository.DeleteTag(taskTag);
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}