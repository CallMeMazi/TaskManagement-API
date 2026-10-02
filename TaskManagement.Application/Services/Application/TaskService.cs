using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.DTOs.ResponseDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Application.Utilities.Exceptions;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Enums.Types.Application;
using TaskManagement.Domain.Interface.Services;

namespace TaskManagement.Application.Services.Application;
public class TaskService : ITaskService
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskDomainService _taskDomainService;
    private readonly ICommonService _common;

    public TaskService(IUnitOfWork unitOfWork, ITaskDomainService taskDomainService, ICommonService common)
    {
        _uow = unitOfWork;
        _taskDomainService = taskDomainService;
        _common = common;
    }

    // Query methods
    public async Task<TaskDetailsDto> GetTaskByIdAsync(long taskId, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(taskId, false, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        var taskDto = _common.Mapper.Map<TaskDetailsDto>(task);

        return taskDto;
    }

    // Command methods
    public async System.Threading.Tasks.Task CreateTaskAsync(CreateTaskAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithMembersAsync(command.ProjId, false, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("شناسه پروژه نامعتبر است!");

        await _taskDomainService.EnsureCanCreateTaskAsync(project!, command.UserId, ct);

        var task = _common.Mapper.Map<Domain.Entities.BaseEntities.Task>(command);

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
    }
    public async System.Threading.Tasks.Task UpdateTaskAsync(UpdateTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.UpdateTask(command.TaskName, command.TaskDescription, command.TaskDeadLine);
    }
    public async System.Threading.Tasks.Task SoftDeleteTaskAsync(UserTaskAppDto command, CancellationToken ct)
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
    }
    public async System.Threading.Tasks.Task ChangeTaskActivityAsync(ChangeTaskActivityAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.ChangeTaskActivity(command.Activity);
    }
    public async System.Threading.Tasks.Task CancelTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.CancelTask();
    }
    public async System.Threading.Tasks.Task DeadTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.DeadTask();
    }
    public async System.Threading.Tasks.Task FinishTaskAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskStateAsync(task!, command.UserId, ct);

        task!.FinishTask();
    }
    public async System.Threading.Tasks.Task ChangeTaskProgressAsync(ChangeTaskProgressAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureUserHasAdminRoleAsync(task!, command.UserId, ct);

        task!.ChangeTaskProgress(command.TaskProgress);
    }
    public async System.Threading.Tasks.Task ChangeTaskTypeAsync(UserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, true, ct);
        if (task!.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanChangeTaskTypeAsync(task!, command.UserId, ct);

        task!.ChangeTaskType();
    }
    // Task Assignment methods
    public async System.Threading.Tasks.Task AssignUserToTaskAsync(AddRemoveUserTaskAppDto command, CancellationToken ct)
    {
        var task = await _uow.Task.GetByIdAsync(command.TaskId, false, ct);
        if (task.IsNullParameter())
            throw new NotFoundException("شناسه تسک نامعتبر است!");

        await _taskDomainService.EnsureCanAssignUserToTaskAsync(task!, command.OwnerId, ct);

        await CreateTaskAssignmentAsync(command.TaskId, command.UserId, command.ProjId, ct);
    }
    public async System.Threading.Tasks.Task RemoveUserFromTaskAsync(AddRemoveUserTaskAppDto command, CancellationToken ct)
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
            throw new ConflictException("کاربر درحال انجام تسک هست و نمیتوانید آن را حذف کنید!");

        taskAssignment.SoftDelete();
    }
    public async System.Threading.Tasks.Task StartTaskAsync(UserTaskAppDto command, CancellationToken ct)
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
    }
    public async System.Threading.Tasks.Task EndTaskAsync(UserTaskAppDto command, CancellationToken ct)
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
    }

    private async System.Threading.Tasks.Task CheckUserIdsAndCreateTaskAssignmentsAsync(List<long> userIds, List<long> memberIds, long taskId
        , long projectid, CancellationToken ct)
    {
        var projMemberIds = memberIds.ToHashSet();
        var invalid = userIds.FirstOrDefault(u => !projMemberIds.Contains(u));
        if (invalid != 0)
            throw new ValidationFailureException($"کاربر با شناسه {invalid} در پروژه وجود ندارد!");

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
