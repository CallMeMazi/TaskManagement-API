using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.DTOs.ApplicationDTOs.Project;
using TaskManagement.Application.DTOs.SharedDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Main;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Enums.Roles;
using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Application.Services.Main;
public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _uow;
    private readonly ICommonService _commonService;
    private readonly IMapper _mapper;


    public ProjectService(IUnitOfWork uow, ICommonService commonService, IMapper mapper)
    {
        _uow = uow;
        _commonService = commonService;
        _mapper = mapper;
    }


    // Query methods
    public async Task<GeneralResult<ProjectDetailsDto>> GetProjectByIdAsync(int projId, CancellationToken ct)
    {
        if (projId.IsNullParameter() || projId <= 0)
            throw new BadRequestException("شناسه پروژه نامعتبر است!");

        var project = await _uow.Project.GetDtoByIdAsync(projId, ct);

        if (project.IsNullParameter())
            throw new NotFoundException("پروژه ای با این شناسه یافت نشد!");

        return GeneralResult<ProjectDetailsDto>.Success(project)!;
    }

    // Command methods
    public async Task<GeneralResult> CreateProjectAsync(CreateProjectAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetOrgByIdWithMembersAsync(command.OrgId, false, ct);
        if (org.IsNullParameter())
            throw new BadRequestException("اطلاعات ورودی نامعتبر است!");

        var isOwnerInOrg = org!.Members.Any(om =>
            om.UserId == command.CreatorId
            && (om.Role == OrganizationRoles.Admin || om.Role == OrganizationRoles.Owner)
        );
        if (!isOwnerInOrg)
            throw new ForbiddenException("شما دسترسی ندارید!");

        var project = _mapper.Map<Project>(command);

        await _uow.Project.AddAsync(project, ct);
        await _uow.SaveAsync(ct);

        // Check UserIds And Creat ProjectMemberShip
        if (!command.UserIds.IsNullParameter())
        {
            try
            {
                var createProjectMemberResult = await CheckUserIdsAndCreateProjectMemberShipsAsync(
                    org.Members.Select(o => o.Id).Take(project.ProjMaxUsers).ToList(),
                    command.UserIds!,
                    project.Id,
                    org.OwnerId,
                    ct
                );
                if (!createProjectMemberResult.IsSuccess)
                    throw new BadRequestException(
                        "پروژه ساخته شد ولی در افزودن اعضا مشکلی وجود داشت!",
                        errorMessages: new List<string> { createProjectMemberResult.Message }
                    );
            }
            catch (Exception ex)
            {
                throw new BadRequestException(
                    "پروژه ساخته شد ولی در افزودن اعضا مشکلی وجود داشت!",
                    innerException: ex
                );
            }
        }

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> UpdateProjectAsync(UpdateProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات وارد شده نامعتبر است!");

        await CheckOwnerIdAsync(project!, command.OwnerId, ct);

        project!.UpdateProject(command.ProjName, command.ProjDescription);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteProjectAsync(UserProjectAppDto command, CancellationToken ct)
    {
        // This method use SP (Stored Procedure)

        var project = await _uow.Project.GetProjectByIdWithOrgAndCreatorAsync(command.ProjId, false, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی نامعتبر است!");

        await CheckOwnerIdAsync(project!, command.OwnerId, ct);

        if (project!.IsActive
            && (project.ProjStatus == ProjectStatusType.InProgress || project.ProjStatus == ProjectStatusType.Adjournment))
            throw new BadRequestException("ابتدا باید پروژه را به پایان برسانید یا آن را کنسل کنید!");

        await CheckUserPasswordAsync(project, command.OwnerId, command.UserPassword, ct);

        var TaskStatusResult = await CheckTaskStatusAsync(project.Id, ct);
        if (!TaskStatusResult.IsSuccess)
            throw new BadRequestException(TaskStatusResult.Message);

        // Delete Project (SP)
        // Delete All ProjectMemberships By ProjectId (SP)
        // Delete All Tasks By ProjectId (SP)
        // Delete All TaskAssignments By ProjectId (SP)
        // Delete All TaslInfos By TaskId (SP)
        await _uow.Project.SoftDeleteProjectSpAsync(project.Id, ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeProjectActivityAsync(ChangeProjectActivityAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await CheckOwnerIdAsync(project!, command.OwnerId, ct);

        if (project!.IsActive == command.Activity)
            throw new BadRequestException(project.IsActive ? "پروژه در حال حاضر فعال است!" : "پروژه در حال حاضر غیر فعال است!");

        await CheckUserPasswordAsync(project, command.OwnerId, command.UserPassword, ct);

        var TaskStatusResult = await CheckTaskStatusAsync(project.Id, ct);
        if (!TaskStatusResult.IsSuccess)
            throw new BadRequestException(TaskStatusResult.Message);

        project.ChangeProjActivity(command.Activity);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeProjectStatusToInProgressAsync(UserProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAndCreatorAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await CheckOwnerIdAsync(project!, command.OwnerId, ct);

        var TaskStatusResult = await CheckTaskStatusAsync(project!.Id, ct);
        if (!TaskStatusResult.IsSuccess)
            throw new BadRequestException(TaskStatusResult.Message);

        if (project.IsActive)
            throw new BadRequestException("پروژه شما درحال حاضر فعال است!");

        await CheckUserPasswordAsync(project, command.OwnerId, command.UserPassword, ct);

        project.ChangeProjStatusToInProgress();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeProjectStatusToAdjournmentAsync(UserProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAndCreatorAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await CheckOwnerIdAsync(project!, command.OwnerId, ct);

        if (!project!.IsActive)
            throw new BadRequestException("پروژه شما درحال حاضر غیرفعال است!");

        await CheckUserPasswordAsync(project, command.OwnerId, command.UserPassword, ct);

        var TaskStatusResult = await CheckTaskStatusAsync(project.Id, ct);
        if (!TaskStatusResult.IsSuccess)
            throw new BadRequestException(TaskStatusResult.Message);

        project.ChangeProjStatusToAdjournment();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> CancelProjectAsync(UserProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAndCreatorAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await CheckOwnerIdAsync(project!, command.OwnerId, ct);

        await CheckUserPasswordAsync(project!, command.OwnerId, command.UserPassword, ct);

        var TaskStatusResult = await CheckTaskStatusAsync(project!.Id, ct);
        if (!TaskStatusResult.IsSuccess)
            throw new BadRequestException(TaskStatusResult.Message);

        project.CancelProj();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> FinishProjectAsync(UserProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAndCreatorAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await CheckOwnerIdAsync(project!, command.OwnerId, ct);

        await CheckUserPasswordAsync(project!, command.OwnerId, command.UserPassword, ct);

        var TaskStatusResult = await CheckTaskStatusAsync(project!.Id, ct);
        if (!TaskStatusResult.IsSuccess)
            throw new BadRequestException(TaskStatusResult.Message);

        project.FinishProj();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeProjectProgressAsync(ChangeProjectProgressAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await CheckOwnerIdAsync(project!, command.OwnerId, ct);

        project!.ChangeProjProgress(command.ProjectProgress);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    // Project MemberShip methods
    public async Task<GeneralResult> AddUserToProjectAysnc(AddRemoveUserProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAndMembersAsync(command.ProjId, false, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await CheckOwnerIdAsync(project!, command.OwnerId, ct);

        var isUserInProject = project!.ProjMember.Any(pm => pm.UserId == command.UserId);
        if (isUserInProject)
            throw new BadRequestException("کاربر مورد نظر در پروژه وجود دارد!");

        if (project.ProjMaxUsers == project.ProjMember.Count)
            throw new BadRequestException("پروژه شما در حال حاضر پر است و نمیتوانید شخص دیگری را اضافه کنید!");

        var isUserInOrg = await _uow.OrganizationMemberShip.IsEntityExistByFilterAsync(om =>
            om.UserId == command.UserId
            && om.OrgId == project.OrgId,
            ct
        );
        if (!isUserInOrg)
            throw new BadRequestException("گاربر مورد نظر در سازمان وجود ندارد!");

        await CreateProjectMemberShipAsync(project.Id, command.UserId, ProjectRoles.Member, ct);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> RemoveUserFromProjectAsync(AddRemoveUserProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAndMembersAsync(command.ProjId, false, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await CheckOwnerIdAsync(project!, command.OwnerId, ct);

        if (project!.CreatorId == command.UserId)
            throw new BadRequestException("شما مالک پروژه هستید و نمیتوانید آن را ترک کنید!");

        var projectMemberShip = project.ProjMember.FirstOrDefault(pm =>
            pm.UserId == command.UserId
            && (pm.Role == ProjectRoles.Admin || pm.Role == ProjectRoles.Member)
        );
        if (projectMemberShip.IsNullParameter())
            throw new NotFoundException("کاربر مورد نظر در پروژه وجود ندارد!");

        var isUserHasActiveTask = await _uow.Task.IsEntityExistByFilterAsync(t =>
            t.ProjId == project.Id
            && t.TaskStatus == TaskStatusType.InProgress
            && t.Members.Any(ta => ta.UserId == command.UserId),
            ct
        );
        if (isUserHasActiveTask)
            throw new BadRequestException("کاربر مورد نظر در تسک فعال حضور دارد، ابتدا تسک را به اتمام برسانید یا کنسل کنید!");

        projectMemberShip!.SoftDelete();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeUserRoleToAdminAsync(ChangeUserRoleProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAsync(command.ProjId, false, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await CheckOwnerIdAsync(project!, command.OwnerId, ct);

        var ProjectMemberShip = await _uow.ProjectMemberShip.GetByFilterAsync(om =>
            om.UserId == command.UserId
            && om.ProjId == command.ProjId,
            true,
            ct
        );
        if (ProjectMemberShip.IsNullParameter())
            throw new NotFoundException("کاربری با این شناسه در پروژه وجود ندارد!");

        if (ProjectMemberShip!.Role == ProjectRoles.Admin)
            throw new BadRequestException("این کاربر در حال حاضر ادمین پروژه هست!");

        ProjectMemberShip.ChangeUserOrgRole(ProjectRoles.Admin);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeUserRoleToMemberAsync(ChangeUserRoleProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAsync(command.ProjId, false, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await CheckOwnerIdAsync(project!, command.OwnerId, ct);

        var ProjectMemberShip = await _uow.ProjectMemberShip.GetByFilterAsync(om =>
            om.UserId == command.UserId
            && om.ProjId == command.ProjId,
            true,
            ct
        );
        if (ProjectMemberShip.IsNullParameter())
            throw new NotFoundException("کاربری با این شناسه در پروژه وجود ندارد!");

        if (ProjectMemberShip!.Role == ProjectRoles.Member)
            throw new BadRequestException("این کاربر در حال حاضر کاربر ساده پروژه هست!");

        ProjectMemberShip.ChangeUserOrgRole(ProjectRoles.Member);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }

    private async System.Threading.Tasks.Task CheckOwnerIdAsync(Project project, int ownerId, CancellationToken ct)
    {
        if (project!.CreatorId != ownerId && project.Org.OwnerId != ownerId)
            throw new ForbiddenException("شما به این پروژه دسترسی ندارید!");

        var isOwnerInOrg = await _uow.OrganizationMemberShip.IsEntityExistByFilterAsync(om =>
            om.UserId == ownerId
            && om.OrgId == project.OrgId
            && (om.Role == OrganizationRoles.Owner || om.Role == OrganizationRoles.Admin)
            , ct
        );
        if (!isOwnerInOrg)
            throw new ForbiddenException("شما به این پروژه دسترسی ندارید!");
    }
    private async System.Threading.Tasks.Task CheckUserPasswordAsync(Project project, int userId, string password, CancellationToken ct)
    {
        if (project.CreatorId == userId)
        {
            if (!_commonService.Password.Verify(project.Creator.PasswordHash, password))
                throw new BadRequestException("رمز عبور نادرست است!");
        }
        else if (project.Org.OwnerId == userId)
        {
            await _uow.Project.LoadReferenceAsync(project, p => p.Org.Owner, ct);
            if (!_commonService.Password.Verify(project.Org.Owner.PasswordHash, password))
                throw new BadRequestException("رمز عبور نادرست است!");
        }
        else
            throw new ForbiddenException("شما به این پروژه دسترسی ندارید!");
    }
    private async Task<GeneralResult> CheckTaskStatusAsync(int projectId, CancellationToken ct)
    {
        var isProjHasActiveTask = await _uow.Task.IsEntityExistByFilterAsync(t =>
            t.ProjId == projectId
            && (t.IsActive || t.TaskStatus == TaskStatusType.InProgress),
            ct
        );
        if (isProjHasActiveTask)
            return GeneralResult.Failure("شما در پروژه تسک های فعال دارید، ابتدا آن ها را عیرفعال کنید!");

        return GeneralResult.Success();
    }
    private async System.Threading.Tasks.Task CreateProjectMemberShipAsync(int projId, int userId, ProjectRoles role, CancellationToken ct)
    {
        var ProjectMemberShip = new ProjectMemberShip(projId, userId, role);

        await _uow.ProjectMemberShip.AddAsync(ProjectMemberShip, ct);
    }
    private async Task<GeneralResult> CheckUserIdsAndCreateProjectMemberShipsAsync(List<int> memberIds, List<int> userIds, int projId
       , int orgOwnerId, CancellationToken ct)
    {
        var orgMemberIds = memberIds.ToHashSet();
        var invalid = userIds.FirstOrDefault(u => !orgMemberIds.Contains(u));
        if (invalid != 0)
            throw new BadRequestException($"کاربر با شناسه {invalid} در سازمان وجود ندارد!");

        var projectMembers = userIds
            .Where(u => u != orgOwnerId)
            .Select(u => new ProjectMemberShip(projId, u, ProjectRoles.Member))
            .ToList();

        await _uow.ProjectMemberShip.AddRangeAsync(projectMembers, ct);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
}
