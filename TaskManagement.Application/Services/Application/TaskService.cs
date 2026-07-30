using AutoMapper;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
using TaskManagement.Application.DTOs.ResponseDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Interface.Services;

namespace TaskManagement.Application.Services.Application;
public class TaskService : ITaskService
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskDomainService _taskDomainService;
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;


    public TaskService(IUnitOfWork unitOfWork, ITaskDomainService taskDomainService, IEventService eventService
        , IMapper mapper)
    {
        _uow = unitOfWork;
        _taskDomainService = taskDomainService;
        _eventService = eventService;
        _mapper = mapper;
    }


    // Query methods
    public async Task<GeneralResult<TaskDetailsDto>> GetTaskByIdAsync(int taskId, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(taskId, false, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        var taskDto = _mapper.Map<TaskDetailsDto>(task);

        return GeneralResult<TaskDetailsDto>.Success(taskDto);
    }

    // Command methods
    public async Task<GeneralResult> CreateTaskAsync(CreateTaskAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithMembersAsync(command.ProjId, false, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("شناسه پروژه نامعتبر است!");

        await _taskDomainService.EnsureCanCreateTaskAsync(project!, command.UserId, ct);

        var task = _mapper.Map<Domain.Entities.BaseEntities.Task>(command);

        await _uow.Task.AddAsync(task, ct);
        await _uow.SaveAsync(ct);

        // Check UserIds And Creat TaskAssignment
        try
        {
            if (!command.UserIds.IsNullParameter())
            {
                if (command.TaskType == TaskType.Group)
                    await CheckUserIdsAndCreateTaskAssignmentsAsync(
                        command.UserIds!.Take(5).ToList(),
                        project!.ProjMember.Select(p => p.Id).ToList(),
                        task.Id,
                        project.Id,
                        ct
                    );
                else
                    await CreateTaskAssignmentAsync(task.Id, command.UserIds!.First(), command.ProjId, true, ct);
            }
        }
        catch (Exception ex)
        {
            throw new BadRequestException(
                "تسک ساخته شد ولی در افزودن اعضا مشکلی وجود داشت!",
                innerException: ex
            );
        }

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> UpdateTaskAsync(UpdateTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.UpdateTask(command.TaskName, command.TaskDescription, command.TaskDeadLine);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, false, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        // Delete Task (SP)
        // Delete All TaskAssignments By TaskId (SP)
        // Delete All TaslInfos By TaskId (SP)
        await _uow.Task.SoftDeleteTaskSpAsync(task!.Id, ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeTaskActivityAsync(ChangeTaskActivityAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.ChangeTaskActivity(command.Activity);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> CancelTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.CancelTask();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> DeadTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.DeadTask();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> FinishTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.FinishTask();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeTaskProgressAsync(ChangeTaskProgressAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureUserHasAdminRoleAsync(task!, command.UserId, ct);

        task!.ChangeTaskProgress(command.TaskProgress);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeTaskTypeAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task!.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskTypeAsync(task!, command.UserId, ct);

        task!.ChangeTaskType();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    // Task Assignment methods
    public async Task<GeneralResult> AssignUserToTaskAsync(AddRemoveUserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, false, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanAssignUserToTaskAsync(task!, command.OwnerId, ct);

        await CreateTaskAssignmentAsync(command.TaskId, command.UserId, command.ProjId, false, ct);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> RemoveUserFromTaskAsync(AddRemoveUserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, false, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanRemoveUserFromTaskAsync(task!, command.UserId, ct);

        var taskAssignment = await _uow.TaskAssignment.GetByFilterAsync(ta =>
            ta.UserId == command.UserId
            && ta.TaskId == command.TaskId,
            true,
            ct
        );
        if (taskAssignment.IsNullParameter())
            throw new Exception($"The TaskAssignment with {command.UserId} userId and {command.TaskId} taslId was not found!");

        if (taskAssignment!.IsInProgress)
            throw new BadRequestException("کاربر درحال انجام تسک هست و نمیتوانید آن را حذف کنید!");

        taskAssignment.SoftDelete();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> StartTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var taskAssignment = await _uow.TaskAssignment.GetByFilterAsync(ta =>
            ta.UserId == command.UserId
            && ta.TaskId == command.TaskId,
            true,
            ct
        );
        if (taskAssignment.IsNullParameter())
            throw new NotFoundException("اطلاعات نامعتبر است!");

        await _taskDomainService.EnsureCanUserStartTaskAsync(command.TaskId, ct);

        taskAssignment!.ChangeTaskInProgress(true);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> EndTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        var taskAssignment = await _uow.TaskAssignment.GetByFilterAsync(ta =>
            ta.UserId == command.UserId
            && ta.TaskId == command.TaskId,
            true,
            ct
        );
        if (taskAssignment.IsNullParameter())
            throw new NotFoundException("اطلاعات نامعتبر است!");

        taskAssignment!.ChangeTaskInProgress(false);

        // Create taskinfo after ended task (Event)
        await _eventService.PublishCreateTaskInfoAsync
            (new CreateTaskInfoAppDto(command.TaskId, command.UserId, taskAssignment.Id, (DateTime)taskAssignment.LastStartedAt!, DateTime.Now), ct);

        return GeneralResult.Success();
    }

    private async System.Threading.Tasks.Task CheckUserIdsAndCreateTaskAssignmentsAsync(List<int> userIds, List<int> memberIds, int taskId
        , int projectid, CancellationToken ct)
    {
        var projMemberIds = memberIds.ToHashSet();
        var invalid = userIds.FirstOrDefault(u => !projMemberIds.Contains(u));
        if (invalid != 0)
            throw new BadRequestException($"کاربر با شناسه {invalid} در پروژه وجود ندارد!");

        var taskAssignments = userIds
            .Select(u => new TaskAssignment(taskId, u, projectid))
            .ToList();

        await _uow.TaskAssignment.AddRangeAsync(taskAssignments, ct);
        await _uow.SaveAsync(ct);
    }
    private async System.Threading.Tasks.Task CreateTaskAssignmentAsync(int taskId, int userId, int projId
        , bool isSaved, CancellationToken ct)
    {
        var taskAsiignment = new TaskAssignment(taskId, userId, projId);

        await _uow.TaskAssignment.AddAsync(taskAsiignment, ct);

        if (isSaved)
            await _uow.SaveAsync(ct);
    }
}
