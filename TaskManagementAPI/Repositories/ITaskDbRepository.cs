using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories;
using Task = TaskManagementAPI.Models.Task;
using TaskStatus = TaskManagementAPI.Models.TaskStatus;

public interface ITaskDbRepository : IBaseRepository<Task>
{
    Task<IEnumerable<Task>> GetTasksByUserIdAsync(Guid userId); 
    Task<IEnumerable<Task>> GetFilteredTasksAsync(Guid userId, TaskStatus? status, TaskPriority? priority, DateTime? dueDate, int page, int pageSize);
}
