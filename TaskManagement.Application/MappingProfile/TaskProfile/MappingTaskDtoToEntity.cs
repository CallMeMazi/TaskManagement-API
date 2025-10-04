using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Task;

namespace TaskManagement.Application.MappingProfile.TaskProfile;
public class MappingTaskDtoToEntity : Profile
{
    public MappingTaskDtoToEntity()
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
