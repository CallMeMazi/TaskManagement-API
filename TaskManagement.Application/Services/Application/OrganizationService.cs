using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
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
public class OrganizationService : IOrganizationService
{
    private readonly ICommonService _commonService;
    private readonly IOrganizationDomainService _orgDomainService;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;


    public OrganizationService(IUnitOfWork unitOfWork, IOrganizationDomainService orgDomainService, IMapper mapper,
        ICommonService commonService)
    {
        _uow = unitOfWork;
        _orgDomainService = orgDomainService;
        _mapper = mapper;
        _commonService = commonService;
    }


    // Query methods
    public async Task<GeneralResult<OrgDetailsDto>> GetOrgByIdAsync(int id, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByIdAsync(id, false, ct);

        if (org.IsNullParameter())
            throw new NotFoundException("سازمانی با این شناسه یافت نشد!");

        var orgDto = _mapper.Map<OrgDetailsDto>(org);

        return GeneralResult<OrgDetailsDto>.Success(orgDto);
    }
    public async Task<GeneralResult<OrgDetailsDto>> GetOrgByCodeAsync(string orgCode, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByFilterAsync(o => o.OrgCode == orgCode, false, ct);

        if (org.IsNullParameter())
            throw new NotFoundException("سازمانی با این کد یافت نشد!");

        var orgDto = _mapper.Map<OrgDetailsDto>(org);

        return GeneralResult<OrgDetailsDto>.Success(orgDto);
    }

    // command services
    public async Task<GeneralResult> CreateOrgAsync(CreateOrgAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        await _orgDomainService.EnsureCanCreateOrgAsync(command.SecondOrgName, command.OwnerId, ct);

        var org = _mapper.Map<Organization>(command);

        await _uow.Organization.AddAsync(org, ct);
        await _uow.SaveAsync(ct);

        // Create relation between owner(User) and Org
        await CreateOrgMemberShipAsync(org.Id, command.OwnerId, OrganizationRoles.Owner, ct);

        return GeneralResult.Success()!;
    }
    public async Task<GeneralResult> UpdateOrgAsync(UpdateOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByIdAsync(command.OrgId, true, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.UserId)
            throw new ForbiddenException("شما مالک این سازمان نیستید و نمیتوانید آن را ویرایش کنید!");

        await _orgDomainService.EnsureCanUpdateOrgAsync(command.SecondOrgName, org.Id, ct);

        org.UpdateOrg(command.OrgName, command.SecondOrgName, command.OrgDescription);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteOrgAsync(DeleteOrgAppDto command, CancellationToken ct)
    {
        // This method use SP (Stored Procedure)

        var org = await _uow.Organization.GetOrgByIdWithOwnerAsync(command.OrgId, false, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.OwnerId)
            throw new ForbiddenException("شما مالک این سازمان نیستید و نمیتوانید آن را حذف کنید!");

        if (!_commonService.Password.Verify(org.Owner.PasswordHash, command.OwnerPassword))
            throw new BadRequestException("رمز عبور اشتباه است!");

        await _orgDomainService.EnsureCanDeactiveOrgAsync(org.Id, ct);

        // Delete Org (SP)
        // Delete All OrgMemberships By OrgId (SP)
        // Delete All Projects By OrgId (SP)
        // Delete All ProjectMemberships By ProjectId (SP)
        // Delete All Tasks By ProjectId (SP)
        // Delete All TaskAssignments By ProjectId (SP)
        // Delete All TaslInfos By TaskId (SP)
        await _uow.User.SoftDeleteUserSpAsync(org.Id, ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeOrgActivityAsync(ChangeActivityOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetOrgByIdWithOwnerAsync(command.OrgId, false, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.OwnerId)
            throw new ForbiddenException("شما مالک این سازمان نیستید و نمیتوانید آن را ویرایش کنید!");

        if (!_commonService.Password.Verify(org.Owner.PasswordHash, command.OwnerPassword))
            throw new BadRequestException("رمز عبور اشتباه است!");

        if (org.IsActive && !command.Activity)
        {
            await _orgDomainService.EnsureCanDeactiveOrgAsync(org.Id, ct);
        }

        org.ChangeOrgActivity(command.Activity);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    // Org MemberShip methods
    public async Task<GeneralResult> AddUserToOrgAsync(AddUserOrgAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        await _orgDomainService.EnsureCanUserAddToOrgAsync(command.OrgId, command.UserId, ct);

        await CreateOrgMemberShipAsync(command.OrgId, command.UserId, OrganizationRoles.Member, ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> RemoveUserFromOrgAsync(RemoveUserOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByIdAsync(command.OrgId, true, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.OrgOwnerId)
            throw new ForbiddenException("شما مالک این سازمان نیستید و نمیتوانید کاربری را حذف کنید!");

        if (org.OwnerId == command.UserId)
            throw new BadRequestException("شما مالک سازمانن هستید و نمیتوانید آن را ترک کنید!");

        var orgMemberShip = await _uow.OrganizationMemberShip.GetByFilterAsync(om =>
            om.UserId == command.UserId
            && (om.Role == OrganizationRoles.Admin || om.Role == OrganizationRoles.Member)
        );
        if (orgMemberShip.IsNullParameter())
            throw new NotFoundException("کاربر مورد نظر در سازمان وجود ندارد!");

       await _orgDomainService.EnsureCanRemoveUserFromOrgAsync(command.OrgId, command.UserId, ct);

        orgMemberShip!.SoftDelete();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> LeaveUserFromOrgAsync(LeaveUserOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByIdAsync(command.OrgId, true, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId == command.UserId)
            throw new BadRequestException("شما مالک سازمان هستید و نمیتوانید آن را ترک کنید!");

        var orgMemberShip = await _uow.OrganizationMemberShip.GetByFilterAsync(om =>
            om.UserId == command.UserId
            && (om.Role == OrganizationRoles.Admin || om.Role == OrganizationRoles.Member)
        );
        if (orgMemberShip.IsNullParameter())
            throw new NotFoundException("شما در این سازمان حضور ندارید!");

        await _orgDomainService.EnsureCanRemoveUserFromOrgAsync(command.OrgId, command.UserId, ct);

        orgMemberShip!.SoftDelete();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeUserRoleToAdminAsync(ChangeUserRoleOrgAppDto command, CancellationToken ct)
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

        orgMemberShip!.ChangeUserOrgRole(OrganizationRoles.Admin);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeUserRoleToMemberAsync(ChangeUserRoleOrgAppDto command, CancellationToken ct)
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

        orgMemberShip!.ChangeUserOrgRole(OrganizationRoles.Member);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }

    private async System.Threading.Tasks.Task CreateOrgMemberShipAsync(int orgId, int userId, OrganizationRoles role, CancellationToken ct)
    {
        var orgMemberShip = new OrganizationMemberShip(orgId, userId, role);

        await _uow.OrganizationMemberShip.AddAsync(orgMemberShip, ct);
    }
}
