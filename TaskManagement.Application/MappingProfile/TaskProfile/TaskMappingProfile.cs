using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Task;
using TaskManagement.Application.DTOs.SharedDTOs.Task;

namespace TaskManagement.Application.MappingProfile.TaskProfile;
public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        // Command DTOs
        CreateMap<CreateTaskAppDto, Domin.Entities.BaseEntities.Task>().ConstructUsing(src =>
        new Domin.Entities.BaseEntities.Task(
            src.ProjId,
            src.UserId,
            src.TaskName,
            src.TaskDescription,
            src.TaskType,
            src.TaskDeadline
        ));

        // Query DTOs
        CreateMap<System.Threading.Tasks.Task, TaskDetailsDto>();
    }
}
