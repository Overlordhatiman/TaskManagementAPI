using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskDTO taskDto)
        {
            var userId = GetUserId();
            var createdTask = await _taskService.CreateTaskAsync(userId, taskDto);
            return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks([FromQuery] TaskFilterDTO filterDto, int page = 1, int pageSize = 10)
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetTasksAsync(userId, filterDto, page, pageSize);
            return Ok(tasks);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var userId = GetUserId();
            var task = await _taskService.GetTaskByIdAsync(userId, id);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskDTO taskDto)
        {
            var userId = GetUserId();
            var updatedTask = await _taskService.UpdateTaskAsync(userId, id, taskDto);
            return Ok(updatedTask);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = GetUserId();
            await _taskService.DeleteTaskAsync(userId, id);
            return NoContent();
        }
    }
}
