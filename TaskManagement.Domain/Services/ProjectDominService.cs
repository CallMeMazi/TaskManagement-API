using TaskManagement.Common.Exceptions;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Enums.Roles;
using TaskManagement.Domain.Enums.Statuses;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Domain.Interface.Services;

namespace TaskManagement.Domain.Services;

public class ProjectDomainService : IProjectDomainService
{
    private readonly IOrganizationMemberShipRepository _orgMemberShipRepository;
    private readonly ITaskRepository _taskRepository;


    public ProjectDomainService(IOrganizationMemberShipRepository orgMemberShipRepository, ITaskRepository taskRepository)
    {
        _orgMemberShipRepository = orgMemberShipRepository;
        _taskRepository = taskRepository;
    }


    public async System.Threading.Tasks.Task EnsureUserHasProjectAccessAsync(int ownerId, int orgId, CancellationToken ct)
    {
        var isOwnerInOrg = await _orgMemberShipRepository.IsEntityExistByFilterAsync(om =>
            om.UserId == ownerId
            && om.OrgId == orgId
            && (om.Role == OrganizationRoles.Owner || om.Role == OrganizationRoles.Admin),
            ct
        );
        if (!isOwnerInOrg)
            throw new ForbiddenException("شما به این پروژه دسترسی ندارید!");
    }
    public async System.Threading.Tasks.Task CheakProjectActiveTaskAsync(int projectId, CancellationToken ct)
    {
        var isProjHasActiveTask = await _taskRepository.IsEntityExistByFilterAsync(t =>
            t.ProjId == projectId
            && (t.IsActive || t.TaskStatus == TaskStatusType.InProgress),
            ct
        );
        if (isProjHasActiveTask)
            throw new BadRequestException("شما در پروژه تسک های فعال دارید، ابتدا آن ها را عیرفعال کنید!");
    }
    // Project Member Ship methods
    public async System.Threading.Tasks.Task EnsureCanAddUserToProjectAsync(Project project, int userId, int orgId, CancellationToken ct)
    {
        if (project.ProjMaxUsers == project.ProjMember.Count)
            throw new BadRequestException("پروژه شما در حال حاضر پر است و نمیتوانید شخص دیگری را اضافه کنید!");

        var isUserInOrg = await _orgMemberShipRepository.IsEntityExistByFilterAsync(om =>
            om.UserId == userId
            && om.OrgId == orgId,
            ct
        );
        if (!isUserInOrg)
            throw new BadRequestException("گاربر مورد نظر در سازمان وجود ندارد!");
    }
    public async System.Threading.Tasks.Task EnsureCanRemoveUserFromProjectAsync(Project project, int userId, CancellationToken ct)
    {
        if (project!.CreatorId == userId)
            throw new BadRequestException("شما مالک پروژه هستید و نمیتوانید آن را ترک کنید!");

        var isUserHasActiveTask = await _taskRepository.IsEntityExistByFilterAsync(t =>
            t.ProjId == project.Id
            && t.TaskStatus == TaskStatusType.InProgress
            && t.Members.Any(ta => ta.UserId == userId),
            ct
        );
        if (isUserHasActiveTask)
            throw new BadRequestException("کاربر مورد نظر در تسک فعال حضور دارد، ابتدا تسک را به اتمام برسانید یا کنسل کنید!");
    }
}
