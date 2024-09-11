using TaskManagementAPI.Models;

public class TaskDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskManagementAPI.Models.TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
}
