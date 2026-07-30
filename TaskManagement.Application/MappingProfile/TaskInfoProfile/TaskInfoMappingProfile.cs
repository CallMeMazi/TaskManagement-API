using AutoMapper;
using TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
using TaskManagement.Application.DTOs.ResponseDTOs.TaskInfo;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.TaskInfoProfile;
public class TaskInfoMappingProfile : Profile
{
    public TaskInfoMappingProfile()
    {
        // Command DTOs
        CreateMap<CreateTaskInfoAppDto, TaskInfo>().ConstructUsing(src =>
        new TaskInfo(
            src.TaskId,
            src.UserId,
            src.TaskAssignmentId,
            src.StartedTaskAt,
            src.EndedTaskAt
        ));

        // Query DTOs
        CreateMap<TaskInfo, TaskInfoDetailsDto>();
    }
}
