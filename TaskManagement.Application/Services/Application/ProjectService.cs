using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Project;
using TaskManagement.Application.DTOs.SharedDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Enums.Roles;
using TaskManagement.Domin.Enums.Statuses;
using TaskManagement.Domin.Interface.Services;

namespace TaskManagement.Application.Services.Application;
public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectDominService _projectDominService;
    private readonly ICommonService _commonService;
    private readonly IMapper _mapper;


    public ProjectService(IUnitOfWork uow, IProjectDominService projectDominService, ICommonService commonService
        , IMapper mapper)
    {
        _uow = uow;
        _projectDominService = projectDominService;
        _commonService = commonService;
        _mapper = mapper;
    }


    // Query methods
    public async Task<GeneralResult<ProjectDetailsDto>> GetProjectByIdAsync(int projId, CancellationToken ct)
    {
        var project = await _uow.Project.GetByIdAsync(projId, false, ct);

        if (project.IsNullParameter())
            throw new NotFoundException("پروژه ای با این شناسه یافت نشد!");

        var projectDto = _mapper.Map<ProjectDetailsDto>(project);

        return GeneralResult<ProjectDetailsDto>.Success(projectDto);
    }

    // Command methods
    public async Task<GeneralResult> CreateProjectAsync(CreateProjectAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

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

        await CreateProjectMemberShipAsync(project.Id, command.CreatorId, ProjectRoles.Creator, ct);

        // Check UserIds And Creat ProjectMemberShip
        if (!command.UserIds.IsNullParameter())
        {
            await CheckUserIdsAndCreateProjMemberShipsAsync(
                org.Members.Select(om => om.UserId).ToList(),
                command.UserIds!.Where(id => id != org.OwnerId && id != command.CreatorId).Take(project.ProjMaxUsers).ToList(),
                project.Id,
                ct
            );
        }

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> UpdateProjectAsync(UpdateProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetByIdAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات وارد شده نامعتبر است!");

        await _projectDominService.EnsureUserHasProjectAccessAsync(command.OwnerId, project!.OrgId, ct);

        project!.UpdateProject(command.ProjName, command.ProjDescription);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteProjectAsync(UserProjectAppDto command, CancellationToken ct)
    {
        // This method use SP (Stored Procedure)

        var project = await _uow.Project.GetProjectByIdWithOrgAsync(command.ProjId, false, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی نامعتبر است!");

        await _projectDominService.EnsureUserHasProjectAccessAsync(command.OwnerId, project!.OrgId, ct);

        if (project!.IsActive
            && (project.ProjStatus == ProjectStatusType.InProgress || project.ProjStatus == ProjectStatusType.Adjournment))
            throw new BadRequestException("ابتدا باید پروژه را به پایان برسانید یا آن را کنسل کنید!");

        await CheckUserPasswordAsync(project, command.OwnerId, command.UserPassword, ct);

        await _projectDominService.CheakProjectActiveTaskAsync(project.Id, ct);

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

        await _projectDominService.EnsureUserHasProjectAccessAsync(command.OwnerId, project!.OrgId, ct);

        if (project!.IsActive == command.Activity)
            throw new BadRequestException(project.IsActive ? "پروژه در حال حاضر فعال است!" : "پروژه در حال حاضر غیر فعال است!");

        await CheckUserPasswordAsync(project, command.OwnerId, command.UserPassword, ct);

        await _projectDominService.CheakProjectActiveTaskAsync(project.Id, ct);

        project.ChangeProjActivity(command.Activity);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeProjectStatusToInProgressAsync(UserProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await _projectDominService.EnsureUserHasProjectAccessAsync(command.OwnerId, project!.OrgId, ct);

        if (project.IsActive)
            throw new BadRequestException("پروژه شما درحال حاضر فعال است!");

        await CheckUserPasswordAsync(project, command.OwnerId, command.UserPassword, ct);

        await _projectDominService.CheakProjectActiveTaskAsync(project.Id, ct);

        project.ChangeProjStatusToInProgress();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeProjectStatusToAdjournmentAsync(UserProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await _projectDominService.EnsureUserHasProjectAccessAsync(command.OwnerId, project!.OrgId, ct);

        if (!project!.IsActive)
            throw new BadRequestException("پروژه شما درحال حاضر غیرفعال است!");

        await CheckUserPasswordAsync(project, command.OwnerId, command.UserPassword, ct);

        await _projectDominService.CheakProjectActiveTaskAsync(project.Id, ct);

        project.ChangeProjStatusToAdjournment();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> CancelProjectAsync(UserProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await _projectDominService.EnsureUserHasProjectAccessAsync(command.OwnerId, project!.OrgId, ct);

        await CheckUserPasswordAsync(project!, command.OwnerId, command.UserPassword, ct);

        await _projectDominService.CheakProjectActiveTaskAsync(project.Id, ct);

        project.CancelProj();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> FinishProjectAsync(UserProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithOrgAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await _projectDominService.EnsureUserHasProjectAccessAsync(command.OwnerId, project!.OrgId, ct);

        await CheckUserPasswordAsync(project!, command.OwnerId, command.UserPassword, ct);

        await _projectDominService.CheakProjectActiveTaskAsync(project.Id, ct);

        project.FinishProj();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeProjectProgressAsync(ChangeProjectProgressAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetByIdAsync(command.ProjId, true, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await _projectDominService.EnsureUserHasProjectAccessAsync(command.OwnerId, project!.OrgId, ct);

        project!.ChangeProjProgress(command.ProjectProgress);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    // Project MemberShip methods
    public async Task<GeneralResult> AddUserToProjectAysnc(AddRemoveUserProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithMembersAsync(command.ProjId, false, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await _projectDominService.EnsureUserHasProjectAccessAsync(command.OwnerId, project!.OrgId, ct);

        var isUserInProject = project!.ProjMember.Any(pm => pm.UserId == command.UserId);
        if (isUserInProject)
            throw new BadRequestException("کاربر مورد نظر در پروژه وجود دارد!");

        await _projectDominService.EnsureCanAddUserToProjectAsync(project, command.UserId, project.OrgId, ct);

        await CreateProjectMemberShipAsync(project.Id, command.UserId, ProjectRoles.Member, ct);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> RemoveUserFromProjectAsync(AddRemoveUserProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetProjectByIdWithMembersAsync(command.ProjId, false, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await _projectDominService.EnsureUserHasProjectAccessAsync(command.OwnerId, project!.OrgId, ct);


        var projectMemberShip = project.ProjMember.FirstOrDefault(pm =>
            pm.UserId == command.UserId
            && (pm.Role == ProjectRoles.Admin || pm.Role == ProjectRoles.Member)
        );
        if (projectMemberShip.IsNullParameter())
            throw new NotFoundException("کاربر مورد نظر در پروژه وجود ندارد!");

        await _projectDominService.EnsureCanRemoveUserFromProjectAsync(project, command.UserId, ct);

        projectMemberShip!.SoftDelete();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeUserRoleToAdminAsync(ChangeUserRoleProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetByIdAsync(command.ProjId, false, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await _projectDominService.EnsureUserHasProjectAccessAsync(command.OwnerId, project!.OrgId, ct);

        var ProjectMemberShip = await _uow.ProjectMemberShip.GetByFilterAsync(om =>
            om.UserId == command.UserId
            && om.ProjId == command.ProjId,
            true,
            ct
        );
        if (ProjectMemberShip.IsNullParameter())
            throw new NotFoundException("کاربری با این شناسه در پروژه وجود ندارد!");

        ProjectMemberShip!.ChangeUserOrgRole(ProjectRoles.Admin);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeUserRoleToMemberAsync(ChangeUserRoleProjectAppDto command, CancellationToken ct)
    {
        var project = await _uow.Project.GetByIdAsync(command.ProjId, false, ct);
        if (project.IsNullParameter())
            throw new NotFoundException("اطلاعات ورودی مامعتبر است!");

        await _projectDominService.EnsureUserHasProjectAccessAsync(command.OwnerId, project!.OrgId, ct);

        var ProjectMemberShip = await _uow.ProjectMemberShip.GetByFilterAsync(om =>
            om.UserId == command.UserId
            && om.ProjId == command.ProjId,
            true,
            ct
        );
        if (ProjectMemberShip.IsNullParameter())
            throw new NotFoundException("کاربری با این شناسه در پروژه وجود ندارد!");

        ProjectMemberShip!.ChangeUserOrgRole(ProjectRoles.Member);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }

    private async System.Threading.Tasks.Task CreateProjectMemberShipAsync(int projId, int userId, ProjectRoles role, CancellationToken ct)
    {
        var ProjectMemberShip = new ProjectMemberShip(projId, userId, role);

        await _uow.ProjectMemberShip.AddAsync(ProjectMemberShip, ct);
    }
    private async System.Threading.Tasks.Task CheckUserIdsAndCreateProjMemberShipsAsync(List<int> memberIds, List<int> userIds, int projId
       , CancellationToken ct)
    {
        try
        {
            var orgMemberIds = memberIds.ToHashSet();
            var invalid = userIds.FirstOrDefault(u => !orgMemberIds.Contains(u));
            if (invalid != 0)
                throw new BadRequestException($"کاربر با شناسه {invalid} در سازمان وجود ندارد!");

            var projectMembers = userIds
                .Select(u => new ProjectMemberShip(projId, u, ProjectRoles.Member))
                .ToList();

            await _uow.ProjectMemberShip.AddRangeAsync(projectMembers, ct);
        }
        catch (Exception ex)
        {
            throw new BadRequestException(
                "پروژه ساخته شد ولی در افزودن اعضا مشکلی وجود داشت!",
                innerException: ex
            );
        }
    }
    private async System.Threading.Tasks.Task CheckUserPasswordAsync(Project project, int userId, string password, CancellationToken ct)
    {
        if (project.CreatorId == userId)
        {
            await _uow.Project.LoadReferenceAsync(project, p => p.Creator, ct);
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
}
