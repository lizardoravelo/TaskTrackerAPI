using AutoMapper;
using TaskTrackerAPI.DTOs;
using TaskTrackerAPI.Models;
using Task = TaskTrackerAPI.Models.Task;

namespace TaskTrackerAPI.Profiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectReadDto>();
            CreateMap<ProjectCreateDto, Project>();
            CreateMap<ProjectUpdateDto, Project>();
            CreateMap<TaskCreateDto, Task>();
            CreateMap<TaskUpdateDto, Task>();
            CreateMap<Task, TaskReadDto>();
        }
    }
}
