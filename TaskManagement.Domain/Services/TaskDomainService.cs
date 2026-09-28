using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Enums.Roles;
using TaskManagement.Domain.Enums.Statuses;
using TaskManagement.Domain.Enums.Types.Application;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Domain.Interface.Services;
using TaskManagement.Domain.Utilities.Exceptions;

namespace TaskManagement.Domain.Services;
public class TaskDomainService : ITaskDomainService
{
    private readonly IOrganizationMemberShipRepository _organizationMemberShipRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly ITaskAssignmentRepository _taskAssignmentRepository;
    private readonly IProjectRepository _projectRepository;


    public TaskDomainService(ITaskRepository taskRepository, ITaskAssignmentRepository taskAssignmentRepository
        , IOrganizationMemberShipRepository organizationMemberShipRepository, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _taskAssignmentRepository = taskAssignmentRepository;
        _organizationMemberShipRepository = organizationMemberShipRepository;
        _projectRepository = projectRepository;
    }


    public async System.Threading.Tasks.Task EnsureUserHasAdminRoleAsync(Entities.BaseEntities.Task task, long userId, CancellationToken ct)
    {
        await CheckUserAdminRoleAsync(task, userId, ct);
    }
    public async System.Threading.Tasks.Task EnsureCanCreateTaskAsync(Project project, long userId, CancellationToken ct)
    {
        var isUserHasAccess = project.ProjMember.Any(pm =>
            pm.UserId == userId
            && (pm.Role == ProjectRole.Admin || pm.Role == ProjectRole.Creator)
        );
        if (!isUserHasAccess)
        {
            var isUserOrgOwner = await _organizationMemberShipRepository.IsEntityExistByFilterAsync(om =>
                om.UserId == userId
                && om.OrgId == project.OrgId
                && om.Role == OrganizationRole.Owner,
                ct
            );
            if (!isUserOrgOwner)
                throw new DomainAuthorizationException("شما دسترسی ندارید!");
        }

        var projTaskCount = await _taskRepository.GetCountByFilterAsync(t =>
            t.ProjId == project.Id
            && t.TaskStatus == TaskStatusType.InProgress,
            ct
        );
        if (project.ProjMaxTasks > projTaskCount)
            throw new DomainLogicalException("پروژه شما به سقف تسک ها رسیده و نمیتوانید تسک جدید اضافه کنید!");
    }
    public async System.Threading.Tasks.Task EnsureCanChangeTaskStateAsync(Entities.BaseEntities.Task task, long userId, CancellationToken ct)
    {
        await CheckUserAdminRoleAsync(task, userId, ct);

        if (!task.IsActive)
            return;

        var isTaskInProgress = await _taskAssignmentRepository.IsEntityExistByFilterAsync(ta =>
            ta.TaskId == task.Id
            && ta.IsInProgress,
            ct
        );
        if (isTaskInProgress)
            throw new DomainLogicalException("تسک شما درحال انجام است و نمیتوانید وضعیت آن را تغییر دهید!");
    }
    public async System.Threading.Tasks.Task EnsureCanChangeTaskTypeAsync(Entities.BaseEntities.Task task, long userId, CancellationToken ct)
    {
        await CheckUserAdminRoleAsync(task, userId, ct);

        var TaskAssigningCount = await _taskAssignmentRepository.GetCountByFilterAsync(ta =>
            ta.TaskId == task.Id,
            ct
        );
        if (TaskAssigningCount > 1 && task.TaskType == TaskType.Group)
            throw new DomainLogicalException("اگر میخواهید تایپ تسک را به گروهی تغییر دهید باید فقط یک نفر را اختصاص دهید و بقیه افراد را حذف کنید!");
    }
    // Task Assignment methods
    public async System.Threading.Tasks.Task EnsureCanAssignUserToTaskAsync(Entities.BaseEntities.Task task, long userId, CancellationToken ct)
    {
        await CheckUserAdminRoleAsync(task, userId, ct);

        if (!task.IsActive)
            throw new DomainLogicalException("تسک غیرفعال است و نمیتوانید کسی را به آن اضافه کنید!");

        if (task.TaskType == TaskType.Single)
            throw new DomainLogicalException("نمیتوانید به تسگی گه منفرد هست فرد دیگری را اضافه کنید!");

        var TaskAssigningCount = await _taskAssignmentRepository.GetCountByFilterAsync(ta =>
            ta.TaskId == task.Id,
            ct
        );
        if (TaskAssigningCount == 5)
            throw new DomainLogicalException("نمیتوانید تسک را به بیشتر از 5 نفر اختصاص دهید!");
    }
    public async System.Threading.Tasks.Task EnsureCanRemoveUserFromTaskAsync(Entities.BaseEntities.Task task, long userId, CancellationToken ct)
    {
        await CheckUserAdminRoleAsync(task, userId, ct);

        if (!task.IsActive)
            throw new DomainLogicalException("تسک غیرفعال است و نمیتوانید کسی را به آن اضافه کنید!");
    }
    public async System.Threading.Tasks.Task EnsureCanUserStartTaskAsync(long taskId, CancellationToken ct)
    {
        var isTaskActive = await _taskRepository.IsEntityExistByFilterAsync(t =>
            t.Id == taskId
            && t.IsActive,
            ct
        );
        if (!isTaskActive)
            throw new DomainLogicalException("تسک غیرفعال است!");
    }

    private async System.Threading.Tasks.Task CheckUserAdminRoleAsync(Entities.BaseEntities.Task task, long userId, CancellationToken ct)
    {
        if (task.CreatorId != userId)
        {
            var isUserCreatorProject = await _projectRepository.IsEntityExistByFilterAsync(p =>
                p.Id == task.ProjId
                && p.CreatorId == userId,
                ct
            );
            if (!isUserCreatorProject)
            {
                var orgId = await _projectRepository.GetFieldByIdAsync(task.ProjId, p => p.OrgId, ct);
                var isUserOwnerOrg = await _organizationMemberShipRepository.IsEntityExistByFilterAsync(om =>
                    om.UserId == userId
                    && om.OrgId == orgId,
                    ct
                );
                if (!isUserOwnerOrg)
                    throw new DomainAuthorizationException("شما دسترسی ندارید!");
            }
        }
    }
}
