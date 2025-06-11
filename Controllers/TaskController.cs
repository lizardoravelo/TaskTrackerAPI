using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTrackerAPI.Data;
using TaskTrackerAPI.DTOs;
using Task = TaskTrackerAPI.Models.Task;
using TaskTrackerAPI.Services;

namespace TaskTrackerAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskTrackerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public TaskController(TaskTrackerDbContext context, IMapper mapper, IUserContext userContext)
        {
            _context = context;
            _mapper = mapper;
            _userContext = userContext;
        }

        [HttpGet("projects/{projectId}/tasks")]
        [Authorize]
        [ProducesResponseType(typeof(ProjectReadDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult GetByProject(int projectId)
        {
            var userId = _userContext.UserId;

            var project = _context.Projects.FirstOrDefault(p => p.Id == projectId && p.UserId == userId);
            if (project == null) return NotFound();

            var tasks = _context.Tasks.Where(t => t.ProjectId == projectId).ToList();
            return Ok(_mapper.Map<IEnumerable<TaskReadDto>>(tasks));
        }

        [HttpPost("projects/{projectId}/tasks")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult Create(int projectId, TaskCreateDto dto)
        {
            var userId = _userContext.UserId;

            var project = _context.Projects.FirstOrDefault(p => p.Id == projectId && p.UserId == userId);
            if (project == null) return NotFound();

            var task = _mapper.Map<Task>(dto);
            task.ProjectId = projectId;
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            _context.Tasks.Add(task);
            _context.SaveChanges();

            var readDto = _mapper.Map<TaskReadDto>(task);
            return Ok(new { message= "Task created successfully", payload = readDto });
        }

        [HttpPut("tasks/{id}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult Update(int id, TaskUpdateDto dto)
        {
            var userId = _userContext.UserId;

            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound("Task Not Found");

            var project = _context.Projects.FirstOrDefault(p => p.Id == task.ProjectId && p.UserId == userId);
            if (project == null) return Unauthorized("you do not have access to this task");

            _mapper.Map(dto, task);
            task.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            var readDto = _mapper.Map<TaskReadDto>(task);
            return Ok(new { message = "Task updated successfully", payload = readDto });
        }

        [HttpDelete("tasks/{id}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult Delete(int id)
        {
            var userId = _userContext.UserId;

            var task = _context.Tasks.FirstOrDefault(task => task.Id == id);
            if (task == null) return NotFound("Task not found");

            var project = _context.Projects.FirstOrDefault(p => p.Id == task.ProjectId && p.UserId == userId);
            if (project == null) return Unauthorized("you do not have access to this task");

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return Ok(new { message = "Task deleted successfully"});
        }
    }
}
