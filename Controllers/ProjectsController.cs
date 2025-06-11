using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTrackerAPI.Data;
using TaskTrackerAPI.DTOs;
using TaskTrackerAPI.Models;
using TaskTrackerAPI.Services;

namespace TaskTrackerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly TaskTrackerDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        public ProjectsController(TaskTrackerDbContext context, IMapper mapper, IUserContext userContext)
        {
            _context = context;
            _mapper = mapper;
            _userContext = userContext;
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ProjectReadDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult GetAll()
        {
            var userId = _userContext.UserId;

            var projects = _context.Projects.Where(p => p.UserId == userId).ToList();
            var projectsDtos = _mapper.Map<IEnumerable<ProjectReadDto>>(projects);
            return Ok(projectsDtos);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ProjectReadDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult GetById(int id)
        {
            var userId = _userContext.UserId;

            var projects = _context.Projects.FirstOrDefault(p => p.UserId == userId && p.Id == id);
            if(projects == null) return NotFound(); 

            var projectsDtos = _mapper.Map<ProjectReadDto>(projects);
            return Ok(projectsDtos);
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ProjectCreateDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult Create(ProjectCreateDto dto)
        {
            var userId = _userContext.UserId;

            var project = _mapper.Map<Project>(dto);
            project.UserId = userId!;
            project.CreatedAt = DateTime.UtcNow;
            project.UpdatedAt = DateTime.UtcNow;

            _context.Projects.Add(project);
            _context.SaveChanges();

            var readDto = _mapper.Map<ProjectReadDto>(project);
            return CreatedAtAction(nameof(GetAll), new { id = readDto.Id }, readDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ProjectUpdateDto),200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult Update(int id, ProjectUpdateDto dto)
        {
            var userId = _userContext.UserId;

            var project = _context.Projects.FirstOrDefault (p => p.Id == id && p.UserId == userId);
            if (project == null) return NotFound();

            _mapper.Map(dto,project);
            project.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok(new { message = "Project updated successfully." });
        }


        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public IActionResult Delete(int id)
        {
            var userId = _userContext.UserId;

            var project = _context.Projects.FirstOrDefault(p => p.Id == id && p.UserId == userId);
            if (project == null) return NotFound();

            _context.Projects.Remove(project);
            _context.SaveChanges();

            return Ok(new {message = "Project deleted successfully."});
        }

    }
}
