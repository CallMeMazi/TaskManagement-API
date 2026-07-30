using AutoMapper;
using TaskManagement.Application.DTOs.ResponseDTOs.TaskInfo;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.TaskInfoProfile;
public class TaskInfoMappingProfile : Profile
{
    public TaskInfoMappingProfile()
    {
        // Command DTOs
        CreateMap<TaskAssignment, TaskInfo>().ConstructUsing(src =>
        new TaskInfo(
            src.TaskId,
            src.UserId,
            src.Id,
            (DateTime)src.LastStartedAt!,
            DateTime.Now
        ));

        // Query DTOs
        CreateMap<TaskInfo, TaskInfoDetailsDto>();
    }
}
