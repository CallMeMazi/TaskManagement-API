using AutoMapper;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.DTOs.ResponseDTOs.Task;
using TaskManagement.Application.Features.Task.Command.AssignUserToTask;
using TaskManagement.Application.Features.Task.Command.ChangeTaskActivity;
using TaskManagement.Application.Features.Task.Command.ChangeTaskProgress;
using TaskManagement.Application.Features.Task.Command.ChangeTaskStatus;
using TaskManagement.Application.Features.Task.Command.ChangeTaskType;
using TaskManagement.Application.Features.Task.Command.CreateTask;
using TaskManagement.Application.Features.Task.Command.DeleteTask;
using TaskManagement.Application.Features.Task.Command.EndTask;
using TaskManagement.Application.Features.Task.Command.RemoveUserFromTask;
using TaskManagement.Application.Features.Task.Command.StartTask;
using TaskManagement.Application.Features.Task.Command.UpdateTask;

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
        CreateMap<Task, TaskDetailsDto>();

        // MediatR Mapping
        CreateMap<CreateTaskCommand, CreateTaskAppDto>();
        CreateMap<UpdateTaskCommand, UpdateTaskAppDto>();
        CreateMap<DeleteTaskCommand, UserTaskAppDto>();
        CreateMap<ChangeTaskActivityCommand, ChangeTaskActivityAppDto>();
        CreateMap<ChangeTaskStatusCommand, UserTaskAppDto>();
        CreateMap<ChangeTaskProgressCommand, ChangeTaskProgressAppDto>();
        CreateMap<ChangeTaskTypeCommand, UserTaskAppDto>();
        CreateMap<AssignUserToTaskCommand, AddRemoveUserTaskAppDto>();
        CreateMap<RemoveUserFromTaskCommand, AddRemoveUserTaskAppDto>();
        CreateMap<StartTaskCommand, UserTaskAppDto>();
        CreateMap<EndTaskCommand, UserTaskAppDto>();
    }
}
