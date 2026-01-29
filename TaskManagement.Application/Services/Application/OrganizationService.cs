using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.DTOs.SharedDTOs.Invitation;
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

namespace TaskManagement.Application.Services.Application;
public class OrganizationService : IOrganizationService
{
    private readonly ICommonService _commonService;
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;


    public OrganizationService(IUnitOfWork unitOfWork, IMapper mapper, ICommonService commonService)
    {
        _uow = unitOfWork;
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

        if (await _uow.Organization.IsEntityExistByFilterAsync(o => o.SecondOrgName == command.SecondOrgName, ct))
            throw new BadRequestException("سازمانی با این نام وجود دارد، لطفا مقدار نام ثانویه را ویرایش کنید!");

        if (await _uow.Organization.IsEntityExistByFilterAsync(o => o.OwnerId == command.OwnerId && o.IsActive, ct))
            throw new BadRequestException("شما نمیتوانید چندین سازمان فعال داشته باشید، لطفا ابتدا سازمان فعلی خود را غیرفعال کیند");

        var userOrgCount = await _uow.Organization.GetCountByFilterAsync(o => o.OwnerId == command.OwnerId, ct);
        if (userOrgCount == 3)
            throw new BadRequestException("نمیوتنید بیشتر از 3 سازمان به نام خود داشته باشید!");

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

        if (await _uow.Organization.IsEntityExistByFilterAsync(o => o.SecondOrgName == command.SecondOrgName && o.Id != org.Id, ct))
            throw new BadRequestException("سازمانی با این نام وجود دارد، لطفا مقدار نام ثانویه را ویرایش کنید!");

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

        var verifyResult = await VerifyOrgAsync(org.Id, ct);
        if (!verifyResult.IsSuccess)
            throw new BadRequestException(verifyResult.Message);

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
            var verifyResult = await VerifyOrgAsync(org.Id, ct);
            if (!verifyResult.IsSuccess)
                throw new BadRequestException(verifyResult.Message);
        }


        org.ChangeOrgActivity(command.Activity);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    // Org MemberShip methods
    public async Task<GeneralResult> AddUserToOrgAsync(AddUserOrgAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        var isUserInOrg = await _uow.OrganizationMemberShip.IsEntityExistByFilterAsync(o =>
            o.OrgId == command.OrgId
            && o.UserId == command.UserId,
            ct
        );
        if (isUserInOrg)
            throw new BadRequestException("شما در این سازمان حضور دارید!");

        await CreateOrgMemberShipAsync(command.OrgId, command.UserId, OrganizationRoles.Member, ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> RemoveUserFromOrgAsync(RemoveUserOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetOrgByIdWithMembersAsync(command.OrgId, true, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.OrgOwnerId)
            throw new ForbiddenException("شما مالک این سازمان نیستید و نمیتوانید کاربری را حذف کنید!");

        if (org.OwnerId == command.UserId)
            throw new BadRequestException("شما مالک سازمانن هستید و نمیتوانید آن را ترک کنید!");

        var orgMemberShip = org.Members.FirstOrDefault(om =>
            om.UserId == command.UserId
            && (om.Role == OrganizationRoles.Admin || om.Role == OrganizationRoles.Member)
        );
        if (orgMemberShip.IsNullParameter())
            throw new NotFoundException("کاربر مورد نظر در سازمان وجود ندارد!");

        var isUserInActiveProj = await _uow.Project.IsEntityExistByFilterAsync(p =>
            p.OrgId == org.Id
            && (p.ProjStatus == ProjectStatusType.InProgress || p.ProjStatus == ProjectStatusType.Adjournment)
            && p.ProjMember.Any(pm => pm.UserId == command.UserId),
            ct
        );
        if (isUserInActiveProj)
            throw new BadRequestException("کاربر مورد نظر در پروژه فعال حضور دارد، ابتدا پروژه را به اتمام برسانید یا کنسل کنید یا کاربر را از پروژه حذف کنید!");

        orgMemberShip!.SoftDelete();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> LeaveUserFromOrgAsync(LeaveUserOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetOrgByIdWithMembersAsync(command.Orgid, true, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId == command.UserId)
            throw new BadRequestException("شما مالک سازمان هستید و نمیتوانید آن را ترک کنید!");

        var orgMemberShip = org.Members.FirstOrDefault(om =>
            om.UserId == command.UserId
            && (om.Role == OrganizationRoles.Admin || om.Role == OrganizationRoles.Member)
        );
        if (orgMemberShip.IsNullParameter())
            throw new NotFoundException("شما در این سازمان حضور ندارید!");

        var isUserInActiveProj = await _uow.Project.IsEntityExistByFilterAsync(p =>
            p.OrgId == org.Id
            && (p.ProjStatus == ProjectStatusType.InProgress || p.ProjStatus == ProjectStatusType.Adjournment)
            && p.ProjMember.Any(pm => pm.UserId == command.UserId),
            ct
        );
        if (isUserInActiveProj)
            throw new BadRequestException("شما نمیتوانید سازمان را ترک کنید، ابتدا از پروژه های فعالی که در آن حضور دارید خارج شوید!");

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

        if (orgMemberShip!.Role == OrganizationRoles.Admin)
            throw new BadRequestException("این کاربر در حال حاضر ادمین سازمان هست!");

        orgMemberShip.ChangeUserOrgRole(OrganizationRoles.Admin);
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

        if (orgMemberShip!.Role == OrganizationRoles.Member)
            throw new BadRequestException("این کاربر در حال حاضر عضو عادی سازمان هست!");

        var isUserHasActiveOrg = await _uow.Project.IsEntityExistByFilterAsync(p =>
            p.CreatorId == command.UserId
            && p.OrgId == command.OrgId
            && (p.ProjStatus == ProjectStatusType.InProgress || p.ProjStatus == ProjectStatusType.Adjournment),
            ct
        );
        if (isUserHasActiveOrg)
            throw new BadRequestException("این ادمین دارای پروژه های فعال است!");

        orgMemberShip.ChangeUserOrgRole(OrganizationRoles.Member);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }

    private async System.Threading.Tasks.Task CreateOrgMemberShipAsync(int orgId, int userId, OrganizationRoles role, CancellationToken ct)
    {
        var orgMemberShip = new OrganizationMemberShip(orgId, userId, role);

        await _uow.OrganizationMemberShip.AddAsync(orgMemberShip, ct);
    }
    private async Task<GeneralResult> VerifyOrgAsync(int orgId, CancellationToken ct)
    {
        var isProjActive = await _uow.Project.IsEntityExistByFilterAsync(p =>
            p.OrgId == orgId
            && (p.ProjStatus == ProjectStatusType.InProgress || p.ProjStatus == ProjectStatusType.Adjournment),
            ct
        );

        if (isProjActive)
            return GeneralResult.Failure("در سازمان شما پروژه فعال وجود دارد، اول آن را کنسل یا به اتمام برسانید!");

        return GeneralResult.Success();
    }
}
