using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Main;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Enums.Roles;

namespace TaskManagement.Application.Services.Main;
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
        if (id.IsNullParameter() || id <= 0)
            throw new BadRequestException("شناسه سازمان نامعتبر است!");

        var org = await _uow.Organization.GetDtoByIdAsync(id, ct);

        if (org.IsNullParameter())
            throw new NotFoundException("سازمانی با این شناسه یافت نشد!");

        return GeneralResult<OrgDetailsDto>.Success(org)!;
    }
    public async Task<GeneralResult<OrgDetailsDto>> GetOrgByCodeAsync(string orgCode, CancellationToken ct)
    {
        if (orgCode.IsNullParameter())
            throw new BadRequestException("کد سازمان خالی است!");

        var org = await _uow.Organization.GetDtoByFilterAsync(o => o.OrgCode == orgCode, ct);

        if (org.IsNullParameter())
            throw new NotFoundException("سازمانی با این کد یافت نشد!");

        return GeneralResult<OrgDetailsDto>.Success(org)!;
    }

    // command services
    public async Task<GeneralResult> CreateOrgAsync(CreateOrgAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        if (await _uow.Organization.IsEntityExistByFilterAsync(o => o.SecondOrgName == command.SecondOrgName, ct))
            throw new BadRequestException("سازمانی با این نام وجود دارد، لطفا مقدار نام ثانویه را ویرایش کنید!");

        if (await _uow.Organization.IsEntityExistByFilterAsync(o => o.OwnerId == command.OwnerId && o.IsActive, ct))
            throw new BadRequestException("شما نمیتوانید چندین سازمان فعال داشته باشید، لطفا ابتدا سازمان فعلی خود را غیرفعال کیند");

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
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید آن را ویرایش کنید!");

        if (await _uow.Organization.IsEntityExistByFilterAsync(o => o.SecondOrgName == command.SecondOrgName && o.Id != org.Id, ct))
            throw new BadRequestException("سازمانی با این نام وجود دارد، لطفا مقدار نام ثانویه را ویرایش کنید!");

        org.UpdateOrg(command.OrgName, command.SecondOrgName, command.OrgDescription);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteOrgAsync(DeleteOrgAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)
        // This method use SP (Stored Procedure)

        var org = await _uow.Organization.GetOrgByIdWithOwnerAsync(command.OrgId, true, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.UserId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید آن را حذف کنید!");

        if (!_commonService.Password.Verify(org.Owner.PasswordHash, command.UserPassword))
            throw new BadRequestException("رمز عبور اشتباه است!");

        org.SoftDelete();
        org.ChangeOrgActivity(false);

        // remove all Users from Org
        await RemoveAllOrgMemberShipsByOrgId(org, false, ct);
        // delete all Orgs Projects (Event)
        // delete all Orgs Tasks (Event)

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeOrgActivityAsync(ChangeActivityOrgAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        var org = await _uow.Organization.GetByIdAsync(command.OrgId, true, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.UserId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید آن را ویرایش کنید!");

        org.ChangeOrgActivity(command.Activity);

        // Inactive Orgs Projets (Event)
        // Inactive Orgs Tasks (Event)

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> AddUserToOrgAsync(AddUserOrgAppDto command, CancellationToken ct)
    {
        var org = await _uow.Organization.GetByIdAsync(command.OrgId, false, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.OrgOwnerId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید کاربری را اضافه کنید!");

        var user = await _uow.User.GetByFilterAsync(u => u.MobileNumber == command.UserMobileNumber, false, ct);
        if (user.IsNullParameter())
            throw new NotFoundException("شماره موبایل کاربر نامعتبر است!");

        var isUserInOrg = await _uow.OrganizationMemberShip.IsEntityExistByFilterAsync(om =>
            om.UserId == user!.Id
            && om.OrgId == org.Id,
            ct
        );
        if (isUserInOrg)
            throw new BadRequestException("این کاربر در سازمان وجود دارد!");

        await CreateOrgMemberShipAsync(org.Id, user!.Id, OrganizationRoles.Member, ct);
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> RemoveUserFromOrgAsync(RemoveUserOrgAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)
        // This method use SP (Stored Procedure)

        var org = await _uow.Organization.GetOrgByIdWithMembersAsync(command.orgId, true, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.orgOwnerId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید کاربری را اضافه کنید!");

        var orgMemberShip = org.Members.FirstOrDefault(om =>
            om.UserId == command.UserId
            && om.OrgId == command.orgId
            && (om.Role == OrganizationRoles.Admin || om.Role == OrganizationRoles.Member)
        );
        if (orgMemberShip.IsNullParameter())
            throw new NotFoundException("داده های ورودی اشتباه است!");

        orgMemberShip!.SoftDelete();

        // remove User from Projects (Event)
        // delete User Tasks (Event)
        // delete User owner Projects (Event)

        return GeneralResult.Success();
    }

    private async System.Threading.Tasks.Task CreateOrgMemberShipAsync(int orgId, int ownerId, OrganizationRoles role, CancellationToken ct)
    {
        var orgMemberShip = new OrganizationMemberShip(orgId, ownerId, role);

        await _uow.OrganizationMemberShip.AddAsync(orgMemberShip, ct);
    }
}
