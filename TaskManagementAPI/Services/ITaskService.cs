using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;

public interface ITaskService
{
    Task<TaskDTO> CreateTaskAsync(Guid userId, TaskDTO taskDto);
    Task<IEnumerable<TaskDTO>> GetTasksAsync(Guid userId, TaskFilterDTO filterDto, int page, int pageSize);
    Task<TaskDTO> GetTaskByIdAsync(Guid userId, Guid taskId);
    Task<TaskDTO> UpdateTaskAsync(Guid userId, Guid taskId, TaskDTO taskDto);
    System.Threading.Tasks.Task DeleteTaskAsync(Guid userId, Guid taskId);
}
