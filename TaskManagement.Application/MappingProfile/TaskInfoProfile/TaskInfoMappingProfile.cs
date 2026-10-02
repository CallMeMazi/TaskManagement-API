using AutoMapper;
using TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
using TaskManagement.Application.DTOs.ResponseDTOs.TaskInfo;
using TaskManagement.Application.Features.Task.Command.EndTask;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Application.MappingProfile.TaskInfoProfile;
public class TaskInfoMappingProfile : Profile
{
    public TaskInfoMappingProfile()
    {
        // Command DTOs
        CreateMap<TaskAssignment, TaskInfo>().ConstructUsing((src, context) =>
        new TaskInfo(
            src.TaskId,
            src.UserId,
            src.Id,
            (string)context.Items[nameof(TaskInfo.TaskInfoDescription)],
            (DateTime)src.LastStartedAt!,
            DateTime.Now
        )).ForAllMembers(opt => opt.Ignore());

        // Query DTOs
        CreateMap<TaskInfo, TaskInfoDetailsDto>();

        // MediatR Mapping
        CreateMap<EndTaskCommand, CreateTaskInfoAppDto>();
    }
}
