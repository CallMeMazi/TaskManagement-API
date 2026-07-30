using AutoMapper;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.DTOs.ResponseDTOs.Task;

namespace TaskManagement.Application.MappingProfile.TaskProfile;
public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        // Command DTOs
        CreateMap<CreateTaskAppDto, Domain.Entities.BaseEntities.Task>().ConstructUsing(src =>
        new Domain.Entities.BaseEntities.Task(
            src.ProjId,
            src.UserId,
            src.TaskName,
            src.TaskDescription,
            src.TaskType,
            src.TaskDeadLine
        ));

        // Query DTOs
        CreateMap<System.Threading.Tasks.Task, TaskDetailsDto>();
    }
}
