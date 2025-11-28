using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.TaskInfo;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.TaskInfoProfile;
public class TaskInfoMappingProfile : Profile
{
    public TaskInfoMappingProfile()
    {
        CreateMap<CreateTaskInfoAppDto, TaskInfo>().ConstructUsing(src =>
        new TaskInfo(
            src.TaskId,
            src.UserId,
            src.TaskAssignmentId
        ));
    }
}
