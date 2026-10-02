using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.DTOs.ResponseDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Application.Utilities.Exceptions;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Enums.Roles;
using TaskManagement.Domain.Interface.Services;

namespace TaskManagement.Application.Services.Application;
public class OrganizationService : IOrganizationService
{
    private readonly ICommonService _common;
    private readonly IOrganizationDomainService _orgDomainService;
    private readonly IUnitOfWork _uow;

    public OrganizationService(IUnitOfWork unitOfWork, IOrganizationDomainService orgDomainService, ICommonService common)
    {
        _uow = unitOfWork;
        _orgDomainService = orgDomainService;
        _common = common;
    }

    // Query methods
    public async Task<OrgDetailsDto> GetOrgByIdAsync(long id, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByIdAsync(id, false, ct);

        if (org.IsNullParameter())
            throw new NotFoundException("سازمانی با این شناسه یافت نشد!");

        var orgDto = _common.Mapper.Map<OrgDetailsDto>(org);

        return orgDto;
    }
    public async Task<OrgDetailsDto> GetOrgByCodeAsync(string orgCode, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByFilterAsync(o => o.OrgCode == orgCode, false, ct);

        if (org.IsNullParameter())
            throw new NotFoundException("سازمانی با این کد یافت نشد!");

        var orgDto = _common.Mapper.Map<OrgDetailsDto>(org);

        return orgDto;
    }

    // command services
    public async System.Threading.Tasks.Task CreateOrgAsync(CreateOrgAppDto command, CancellationToken ct)
    {
        await _orgDomainService.EnsureCanCreateOrgAsync(command.SecondOrgName, command.OwnerId, ct);

        var org = _common.Mapper.Map<Organization>(command);

        await _uow.Organization.AddAsync(org, ct);

        // Create relation between owner(User) and Org
        await CreateOrgMemberShipAsync(org.Id, command.OwnerId, OrganizationRole.Owner, ct);
    }
    public async System.Threading.Tasks.Task UpdateOrgAsync(UpdateOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByIdAsync(command.OrgId, true, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.UserId)
            throw new ForbiddenException("شما مالک این سازمان نیستید و نمیتوانید آن را ویرایش کنید!");

        await _orgDomainService.EnsureCanUpdateOrgAsync(command.SecondOrgName, org.Id, ct);

        org.UpdateOrg(command.OrgName, command.SecondOrgName, command.OrgDescription);
    }
    public async System.Threading.Tasks.Task SoftDeleteOrgAsync(DeleteOrgAppDto command, CancellationToken ct)
    {
        // This method use SP (Stored Procedure)

        var org = await _uow.Organization.GetOrgByIdWithOwnerAsync(command.OrgId, false, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.OwnerId)
            throw new ForbiddenException("شما مالک این سازمان نیستید و نمیتوانید آن را حذف کنید!");

        _common.Password.VerifyAndCheck(org.Owner.PasswordHash, command.OwnerPassword, "رمز عبور اشتباه است!");

        await _orgDomainService.EnsureCanDeactiveOrgAsync(org.Id, ct);

        // Delete Org (SP)
        // Delete All OrgMemberships By OrgId (SP)
        // Delete All OrgInvitation By OrgId (SP)
        // Delete All Projects By OrgId (SP)
        // Delete All ProjectMemberships By ProjectId (SP)
        // Delete All Tasks By ProjectId (SP)
        // Delete All TaskAssignments By ProjectId (SP)
        // Delete All TaslInfos By TaskId (SP)
        await _uow.User.SoftDeleteUserSpAsync(org.Id, ct);
    }
    public async System.Threading.Tasks.Task ChangeOrgActivityAsync(ChangeActivityOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetOrgByIdWithOwnerAsync(command.OrgId, false, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.OwnerId)
            throw new ForbiddenException("شما مالک این سازمان نیستید و نمیتوانید آن را ویرایش کنید!");

        _common.Password.VerifyAndCheck(org.Owner.PasswordHash, command.OwnerPassword, "رمز عبور اشتباه است!");

        if (org.IsActive && !command.Activity)
            await _orgDomainService.EnsureCanDeactiveOrgAsync(org.Id, ct);

        org.ChangeOrgActivity(command.Activity);
    }
    // Org MemberShip methods
    public async System.Threading.Tasks.Task AddUserToOrgAsync(AddUserOrgAppDto command, CancellationToken ct)
    {
        await _orgDomainService.EnsureCanUserAddToOrgAsync(command.OrgId, command.UserId, ct);

        await CreateOrgMemberShipAsync(command.OrgId, command.UserId, OrganizationRole.Member, ct);
    }
    public async System.Threading.Tasks.Task RemoveUserFromOrgAsync(RemoveUserOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByIdAsync(command.OrgId, true, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.OrgOwnerId)
            throw new ForbiddenException("شما مالک این سازمان نیستید و نمیتوانید کاربری را حذف کنید!");

        if (org.OwnerId == command.UserId)
            throw new ConflictException("شما مالک سازمانن هستید و نمیتوانید آن را ترک کنید!");

        var orgMemberShip = await _uow.OrganizationMemberShip.GetByFilterAsync(om =>
            om.UserId == command.UserId
            && (om.Role == OrganizationRole.Admin || om.Role == OrganizationRole.Member)
        );
        if (orgMemberShip.IsNullParameter())
            throw new NotFoundException("کاربر مورد نظر در سازمان وجود ندارد!");

        await _orgDomainService.EnsureCanRemoveUserFromOrgAsync(command.OrgId, command.UserId, ct);

        orgMemberShip!.SoftDelete();
    }
    public async System.Threading.Tasks.Task LeaveUserFromOrgAsync(LeaveUserOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByIdAsync(command.OrgId, true, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId == command.UserId)
            throw new ConflictException("شما مالک سازمان هستید و نمیتوانید آن را ترک کنید!");

        var orgMemberShip = await _uow.OrganizationMemberShip.GetByFilterAsync(om =>
            om.UserId == command.UserId
            && (om.Role == OrganizationRole.Admin || om.Role == OrganizationRole.Member)
        );
        if (orgMemberShip.IsNullParameter())
            throw new NotFoundException("شما در این سازمان حضور ندارید!");

        await _orgDomainService.EnsureCanRemoveUserFromOrgAsync(command.OrgId, command.UserId, ct);

        orgMemberShip!.SoftDelete();
    }
    public async System.Threading.Tasks.Task ChangeUserRoleToAdminAsync(ChangeUserRoleOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByIdAsync(command.OrgId, false, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("سازمانی با این شناسه یافت نشد!");

        if (org!.OwnerId != command.OrgOwnerId)
            throw new ForbiddenException("شما مالک این سازمان نیستید!");

        var orgMemberShip = await _uow.OrganizationMemberShip.GetByFilterAsync(om =>
            om.UserId == command.UserId
            && om.OrgId == command.OrgId,
            true,
            ct
        );
        if (orgMemberShip.IsNullParameter())
            throw new NotFoundException("کاربری با این شناسه در سازمان وجود ندارد!");

        orgMemberShip!.ChangeUserOrgRole(OrganizationRole.Admin);
    }
    public async System.Threading.Tasks.Task ChangeUserRoleToMemberAsync(ChangeUserRoleOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByIdAsync(command.OrgId, false, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("سازمانی با این شناسه یافت نشد!");

        if (org!.OwnerId != command.OrgOwnerId)
            throw new ForbiddenException("شما مالک این سازمان نیستید!");

        var orgMemberShip = await _uow.OrganizationMemberShip.GetByFilterAsync(om =>
            om.UserId == command.UserId
            && om.OrgId == command.OrgId,
            true,
            ct
        );
        if (orgMemberShip.IsNullParameter())
            throw new NotFoundException("کاربری با این شناسه در سازمان وجود ندارد!");

        await _orgDomainService.EnsureCanChangeRoleToMemberAsync(command.UserId, org.Id, ct);

        orgMemberShip!.ChangeUserOrgRole(OrganizationRole.Member);
    }

    private async System.Threading.Tasks.Task CreateOrgMemberShipAsync(long orgId, long userId, OrganizationRole role, CancellationToken ct)
    {
        var orgMemberShip = new OrganizationMemberShip(orgId, userId, role);

        await _uow.OrganizationMemberShip.AddAsync(orgMemberShip, ct);
    }
}
