using AutoMapper;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories;
using TaskManagementAPI.Services;

public class TaskService : ITaskService
{
    private readonly ITaskDbRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TaskService> _logger;

    public TaskService(ITaskDbRepository taskRepository, IMapper mapper, ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _mapper = mapper;
        _logger = logger;
    }

    // Create a new task for the user
    public async Task<TaskDTO> CreateTaskAsync(Guid userId, TaskDTO taskDto)
    {
        try
        {
            var task = _mapper.Map<TaskManagementAPI.Models.Task>(taskDto);
            task.UserId = userId;

            await _taskRepository.AddAsync(task);
            _logger.LogInformation("Task {Title} created for user {UserId}.", task.Title, userId);
            return _mapper.Map<TaskDTO>(task);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating task {Title} for user {UserId}.", taskDto.Title, userId);
            throw;
        }
    }

    // Get tasks for the user with optional filters and pagination
    public async Task<IEnumerable<TaskDTO>> GetTasksAsync(Guid userId, TaskFilterDTO filterDto, int page, int pageSize)
    {
        try
        {
            var tasks = await _taskRepository.GetFilteredTasksAsync(userId, filterDto.Status, filterDto.Priority, filterDto.DueDate, page, pageSize);
            _logger.LogInformation("{TaskCount} tasks retrieved for user {UserId}.", tasks.Count(), userId);
            return _mapper.Map<IEnumerable<TaskDTO>>(tasks);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving tasks for user {UserId}.", userId);
            throw;
        }
    }

    // Get a specific task by ID
    public async Task<TaskDTO> GetTaskByIdAsync(Guid userId, Guid taskId)
    {
        try
        {
            var task = await _taskRepository.GetByIdAsync(taskId);

            if (task == null || task.UserId != userId)
            {
                _logger.LogWarning("Task {TaskId} not found for user {UserId}.", taskId, userId);
                throw new KeyNotFoundException("Task not found.");
            }
            _logger.LogInformation("Task {TaskId} retrieved for user {UserId}.", taskId, userId);
            return _mapper.Map<TaskDTO>(task);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving task {TaskId} for user {UserId}.", taskId, userId);
            throw;
        }
    }

    // Update an existing task
    public async Task<TaskDTO> UpdateTaskAsync(Guid userId, Guid taskId, TaskDTO taskDto)
    {
        try
        {
            var task = await _taskRepository.GetByIdAsync(taskId);

            if (task == null || task.UserId != userId)
            {
                _logger.LogWarning("Task {TaskId} not found for user {UserId}.", taskId, userId);
                throw new KeyNotFoundException("Task not found.");
            }

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.DueDate = taskDto.DueDate;
            task.Status = taskDto.Status;
            task.Priority = taskDto.Priority;
            task.UpdatedAt = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(task);
            _logger.LogInformation("Task {TaskId} updated for user {UserId}.", taskId, userId);

            return _mapper.Map<TaskDTO>(task);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating task {TaskId} for user {UserId}.", taskId, userId);
            throw;
        }
    }

    // Delete a task
    public async System.Threading.Tasks.Task DeleteTaskAsync(Guid userId, Guid taskId)
    {
        try
        {
            var task = await _taskRepository.GetByIdAsync(taskId);

            if (task == null || task.UserId != userId)
            {
                _logger.LogWarning("Task {TaskId} not found for user {UserId}.", taskId, userId);
                throw new KeyNotFoundException("Task not found.");
            }

            await _taskRepository.DeleteAsync(taskId);
            _logger.LogInformation("Task {TaskId} deleted for user {UserId}.", taskId, userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting task {TaskId} for user {UserId}.", taskId, userId);
            throw;
        }
    }
}
