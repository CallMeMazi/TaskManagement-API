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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public OrganizationService(IUnitOfWork unitOfWork, IMapper mapper, ICommonService commonService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _commonService = commonService;
    }


    // query services
    public async Task<GeneralResult<OrgDetailsDto>> GetOrgByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id.IsNullParameter() || id <= 0)
            throw new BadRequestException("شناسه سازمان نامعتبر است!");

        var org = await _unitOfWork.OrganizationRepository.GetOrgDtoByIdAsync(id, cancellationToken);

        if (org.IsNullParameter())
            throw new NotFoundException("سازمانی با این شناسه یافت نشد!");

        return GeneralResult<OrgDetailsDto>.Success(org)!;
    }
    public async Task<GeneralResult<OrgDetailsDto>> GetOrgByCodeAsync(string orgCode, CancellationToken cancellationToken)
    {
        if (orgCode.IsNullParameter())
            throw new BadRequestException("کد سازمان خالی است!");

        var org = await _unitOfWork.OrganizationRepository.GetOrgDtoByFilterAsync(o => o.OrgCode == orgCode, cancellationToken);

        if (org.IsNullParameter())
            throw new NotFoundException("سازمانی با این کد یافت نشد!");

        return GeneralResult<OrgDetailsDto>.Success(org)!;
    }

    // command services
    public async Task<GeneralResult> CreateOrgAsync(CreateOrgAppDto command, CancellationToken cancellationToken)
    {
        // This method is used in transaction (TransAction)

        if (await _unitOfWork.OrganizationRepository.IsOrgExistByFilterAsync(o => o.SecondOrgName == command.SecondOrgName, cancellationToken))
            throw new BadRequestException("سازمانی با این نام وجود دارد، لطفا مقدار نام ثانویه را ویرایش کنید!");

        if (await _unitOfWork.OrganizationRepository.IsOrgExistByFilterAsync(o => o.OwnerId == command.OwnerId && o.IsActive, cancellationToken))
            throw new BadRequestException("شما نمیتوانید چندین سازمان فعال داشته باشید، لطفا ابتدا سازمان فعلی خود را غیرفعال کیند");

        var org = _mapper.Map<Organization>(command);

        await _unitOfWork.OrganizationRepository.AddOrgAsync(org, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        // Create relation between owner(User) and Org
        await CreateOrgMemberShipAsync(org.Id, command.OwnerId, OrganizationRoles.Owner, cancellationToken);

        return GeneralResult.Success()!;
    }
    public async Task<GeneralResult> UpdateOrgAsync(UpdateOrgAppDto command, CancellationToken cancellationToken)
    {
        var org = await _unitOfWork.OrganizationRepository.GetOrgByIdAsync(command.OrgId, true, cancellationToken);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.UserId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید آن را ویرایش کنید!");

        if (await _unitOfWork.OrganizationRepository.IsOrgExistByFilterAsync(o => o.SecondOrgName == command.SecondOrgName, cancellationToken))
            throw new BadRequestException("سازمانی با این نام وجود دارد، لطفا مقدار نام ثانویه را ویرایش کنید!");

        org.UpdateOrg(command.OrgName, command.SecondOrgName, command.OrgDescription);
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteOrgAsync(DeleteOrgAppDto command, CancellationToken cancellationToken)
    {
        // This method is used in transaction (TransAction)

        var org = await _unitOfWork.OrganizationRepository.GetOrgByIdAsync(command.OrgId, true, cancellationToken);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.UserId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید آن را حذف کنید!");

        await _unitOfWork.OrganizationRepository.LoadReferenceAsync(org, o => o.Owner, cancellationToken);

        if (!_commonService.Password.Verify(org.Owner.PasswordHash, command.UserPassword))
            throw new BadRequestException("رمز عبور اشتباه است!");

        org.SoftDelete();
        org.ChangeOrgActivity(false);

        // remove all Users From Org
        await RemoveAllOrgMemberShipsByOrgId(org, false, cancellationToken);
        // delete all Orgs Projects (Event)
        // delete all Orgs Tasks (Event)

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteAllOrgsByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        // This method is used in transaction (TransAction)

        var orgs = await _unitOfWork.OrganizationRepository.GetAllOrgsByFilterAsync(o =>
            o.OwnerId == userId,
            true,
            cancellationToken
        );
        if (orgs.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (orgs!.OwnerId != command.UserId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید آن را حذف کنید!");

        await _unitOfWork.OrganizationRepository.LoadReferenceAsync(orgs, o => o.Owner, cancellationToken);

        if (!_commonService.Password.Verify(orgs.Owner.PasswordHash, command.UserPassword))
            throw new BadRequestException("رمز عبور اشتباه است!");

        orgs.SoftDelete();
        orgs.ChangeOrgActivity(false);

        // remove all Users From Org
        await RemoveAllOrgMemberShipsByOrgId(orgs, false, cancellationToken);
        // delete all Orgs Projects (Event)
        // delete all Orgs Tasks (Event)

        return GeneralResult.Success();
    }

    public async Task<GeneralResult> ChangeOrgActivityAsync(ChangeActivityOrgAppDto command, CancellationToken cancellationToken)
    {
        // This method is used in transaction (TransAction)

        var org = await _unitOfWork.OrganizationRepository.GetOrgByIdAsync(command.OrgId, true, cancellationToken);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.UserId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید آن را ویرایش کنید!");

        org.ChangeOrgActivity(command.Activity);

        // Inactive Orgs Projets (Event)
        // Inactive Orgs Tasks (Event)

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> AddUserToOrgAsync(AddUserOrgAppDto command, CancellationToken cancellationToken)
    {
        var org = await _unitOfWork.OrganizationRepository.GetOrgByIdAsync(command.OrgId, false, cancellationToken);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.OrgOwnerId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید کاربری را اضافه کنید!");

        var user = await _unitOfWork.UserRepository.GetUserByFilterAsync(
            u => u.MobileNumber == command.UserMobileNumber,
            false,
            cancellationToken
        );
        if (user.IsNullParameter())
            throw new NotFoundException("شماره موبایل کاربر نامعتبر است!");

        var isUserInOrg = await _unitOfWork.OrganizationMemberShipRepository
            .IsOrgMembershipExistByFilterAsync(om =>
                om.UserId == user!.Id
                && om.OrgId == org.Id,
                cancellationToken
            );
        if (isUserInOrg)
            throw new BadRequestException("این کاربر در سازمان وجود دارد!");

        await CreateOrgMemberShipAsync(org.Id, user!.Id, OrganizationRoles.Member, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> RemoveUserFromOrgAsync(RemoveUserOrgAppDto command, CancellationToken cancellationToken)
    {
        // This method is used in transaction (TransAction)

        var org = await _unitOfWork.OrganizationRepository.GetOrgByIdAsync(command.orgId, true, cancellationToken);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.orgOwnerId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید کاربری را اضافه کنید!");

        var orgMemberShip = await _unitOfWork.OrganizationMemberShipRepository
            .GetOrgMembershipByFilterAsync(om =>
                om.UserId == command.UserId
                && om.OrgId == command.orgId
                && (om.Role == OrganizationRoles.Admin || om.Role == OrganizationRoles.Member),
                true,
                cancellationToken
            );
        if (orgMemberShip.IsNullParameter())
            throw new NotFoundException("داده های ورودی اشتباه است!");
        
        orgMemberShip!.SoftDelete();

        // remove User from Projects (Event)
        // delete User Tasks (Event)
        // delete User owner Projects (Event)
        
        return GeneralResult.Success();
    }

    private async System.Threading.Tasks.Task CreateOrgMemberShipAsync(int orgId, int ownerId, OrganizationRoles role, CancellationToken cancellationToken)
    {
        var orgMemberShip = new OrganizationMemberShip(orgId, ownerId, role);

        await _unitOfWork.OrganizationMemberShipRepository.AddOrgMembershipAsync(orgMemberShip, cancellationToken);
    }
    private async System.Threading.Tasks.Task RemoveAllOrgMemberShipsByOrgId(Organization org, bool IsSaved, CancellationToken cancellationToken)
    {
        await _unitOfWork.OrganizationRepository.LoadCollectionAsync(org, o => o.Members, cancellationToken);

        if (org.Members.IsNullParameter() || !org!.Members.Any())
            throw new Exception($"The OrganizationMemberShip List is null or empty, in {nameof(RemoveAllOrgMemberShipsByOrgId)} method!");

        foreach (var membership in org.Members)
            membership.SoftDelete();

        if (IsSaved)
            await _unitOfWork.SaveAsync(cancellationToken);
    }
}
