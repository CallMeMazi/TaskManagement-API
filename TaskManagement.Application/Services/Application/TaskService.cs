using AutoMapper;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.DTOs.ResponseDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
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
    private readonly IMapper _mapper;


    public TaskService(IUnitOfWork unitOfWork, ITaskDomainService taskDomainService, IMapper mapper)
    {
        _uow = unitOfWork;
        _taskDomainService = taskDomainService;
        _mapper = mapper;
    }


    // Query methods
    public async Task<GeneralResult<TaskDetailsDto>> GetTaskByIdAsync(long taskId, CancellationToken ct)
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

        // Check UserIds And Creat TaskAssignment
        if (command.TaskType == TaskType.Group)
            await CheckUserIdsAndCreateTaskAssignmentsAsync(
                command.UserIds!.Take(5).ToList(),
                project!.ProjMember.Select(p => p.Id).ToList(),
                task.Id,
                project.Id,
                ct
            );
        else
            await CreateTaskAssignmentAsync(task.Id, command.UserIds!.First(), command.ProjId, ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> UpdateTaskAsync(UpdateTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.UpdateTask(command.TaskName, command.TaskDescription, command.TaskDeadLine);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        // This method use SP (Stored Procedure)

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

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> CancelTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.CancelTask();

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> DeadTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.DeadTask();

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> FinishTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.FinishTask();

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeTaskProgressAsync(ChangeTaskProgressAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureUserHasAdminRoleAsync(task!, command.UserId, ct);

        task!.ChangeTaskProgress(command.TaskProgress);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeTaskTypeAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task!.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskTypeAsync(task!, command.UserId, ct);

        task!.ChangeTaskType();

        return GeneralResult.Success();
    }
    // Task Assignment methods
    public async Task<GeneralResult> AssignUserToTaskAsync(AddRemoveUserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, false, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanAssignUserToTaskAsync(task!, command.OwnerId, ct);

        await CreateTaskAssignmentAsync(command.TaskId, command.UserId, command.ProjId, ct);

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

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> EndTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var taskAssignment = await _uow.TaskAssignment.GetByFilterAsync(ta =>
            ta.UserId == command.UserId
            && ta.TaskId == command.TaskId,
            true,
            ct
        );
        if (taskAssignment.IsNullParameter())
            throw new NotFoundException("اطلاعات نامعتبر است!");

        taskAssignment!.ChangeTaskInProgress(false);

        return GeneralResult.Success();
    }

    private async System.Threading.Tasks.Task CheckUserIdsAndCreateTaskAssignmentsAsync(List<long> userIds, List<long> memberIds, long taskId
        , long projectid, CancellationToken ct)
    {
        var projMemberIds = memberIds.ToHashSet();
        var invalid = userIds.FirstOrDefault(u => !projMemberIds.Contains(u));
        if (invalid != 0)
            throw new BadRequestException($"کاربر با شناسه {invalid} در پروژه وجود ندارد!");

        var taskAssignments = userIds
            .Select(u => new TaskAssignment(taskId, u, projectid))
            .ToList();

        await _uow.TaskAssignment.AddRangeAsync(taskAssignments, ct);
    }
    private async System.Threading.Tasks.Task CreateTaskAssignmentAsync(long taskId, long userId, long projId
        , CancellationToken ct)
    {
        var taskAsiignment = new TaskAssignment(taskId, userId, projId);

        await _uow.TaskAssignment.AddAsync(taskAsiignment, ct);
    }
}
