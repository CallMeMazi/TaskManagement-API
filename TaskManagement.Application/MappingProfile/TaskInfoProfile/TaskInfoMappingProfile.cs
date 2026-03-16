using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.TaskInfo;
using TaskManagement.Application.DTOs.SharedDTOs.TaskInfo;
using TaskManagement.Domin.Entities.BaseEntities;

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
