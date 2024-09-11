using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Data.TaskManagementAPI.Data;
using TaskManagementAPI.Repositories;
using Task = TaskManagementAPI.Models.Task;

public class TaskDbRepository : BaseRepository<Task>, ITaskDbRepository
{
    public TaskDbRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Task>> GetFilteredTasksAsync(Guid userId, TaskManagementAPI.Models.TaskStatus? status, TaskPriority? priority, DateTime? dueDate, int page, int pageSize)
    {
        var query = _context.Tasks.Where(t => t.UserId == userId);

        // Apply filtering if parameters are not null
        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(t => t.Priority == priority.Value);
        }

        if (dueDate.HasValue)
        {
            query = query.Where(t => t.DueDate.HasValue && t.DueDate.Value.Date == dueDate.Value.Date);
        }

        // Apply pagination
        query = query.Skip((page - 1) * pageSize).Take(pageSize);

        return await query.ToListAsync();
    }
    public async Task<IEnumerable<Task>> GetTasksByUserIdAsync(Guid userId)
    {
        return await _context.Tasks.Where(task => task.UserId == userId).ToListAsync();
    }
}
