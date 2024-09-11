using TaskManagementAPI.Models;
using TaskStatus = TaskManagementAPI.Models.TaskStatus;

public class TaskFilterDTO
{
    public TaskStatus? Status { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskPriority? Priority { get; set; }
}
