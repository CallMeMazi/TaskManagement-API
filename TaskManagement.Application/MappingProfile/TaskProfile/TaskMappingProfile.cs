using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Task;

namespace TaskManagement.Application.MappingProfile.TaskProfile;
public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<CreateTaskAppDto, Domin.Entities.BaseEntities.Task>().ConstructUsing(src =>
        new Domin.Entities.BaseEntities.Task(
            src.ProjId,
            src.CreatorId,
            src.TaskName,
            src.TaskDescription,
            src.TaskType,
            src.TaskDeadline
        ));
    }
}
