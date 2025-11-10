using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Application.Interfaces.Repositories;
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
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public OrganizationService(IOrganizationRepository organizationRepository, IUnitOfWork unitOfWork, IMapper mapper
        , ICommonService commonService)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _commonService = commonService;
    }


    // query services
    public async Task<GeneralResult<OrgDetailsDto>> GetOrganizationByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id.IsNullParameter() || id <= 0)
            throw new BadRequestException("شناسه سازمان نامعتبر است!");

        var org = await _unitOfWork.OrganizationRepository.GetOrgDtoByIdAsync(id, cancellationToken);

        if (org.IsNullParameter())
            throw new NotFoundException("سازمانی با این شناسه یافت نشد!");

        return GeneralResult<OrgDetailsDto>.Success(org)!;
    }
    public async Task<GeneralResult<OrgDetailsDto>> GetOrganizationByCodeAsync(string orgCode, CancellationToken cancellationToken)
    {
        if (orgCode.IsNullParameter())
            throw new BadRequestException("کد سازمان خالی است!");

        var org = await _unitOfWork.OrganizationRepository.GetOrgDtoByFilterAsync(o => o.OrgCode == orgCode, cancellationToken);

        if (org.IsNullParameter())
            throw new NotFoundException("سازمانی با این کد یافت نشد!");

        return GeneralResult<OrgDetailsDto>.Success(org)!;
    }

    // command services
    public async Task<GeneralResult> CreateOrganizationAsync(CreateOrgAppDto command, CancellationToken cancellationToken)
    {
        // This method is used in transaction (TransAction)

        if (await _organizationRepository.IsOrgExistByFilterAsync(o => o.SecondOrgName == command.SecondOrgName, cancellationToken))
            throw new BadRequestException("سازمانی با این نام وجود دارد، لطفا مقدار نام ثانویه را ویرایش کنید!");

        if (await _organizationRepository.IsOrgExistByFilterAsync(o => o.OwnerId == command.OwnerId && o.IsActive, cancellationToken))
            throw new BadRequestException("شما نمیتوانید چندین سازمان فعال داشته باشید، لطفا ابتدا سازمان فعلی خود را غیرفعال کیند");

        var org = _mapper.Map<Organization>(command);

        await _organizationRepository.AddOrgAsync(org, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        // Create relation between owner(User) and Org
        await CreateOrgMemberShipAsync(org.Id, command.OwnerId, OrganizationRoles.Owner, cancellationToken);

        return GeneralResult.Success()!;
    }
    public async Task<GeneralResult> UpdateOrganizationAsync(UpdateOrgAppDto command, CancellationToken cancellationToken)
    {
        var org = await _organizationRepository.GetOrgByIdAsync(command.OrgId, true, cancellationToken);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.UserId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید آن را ویرایش کنید!");

        if (await _organizationRepository.IsOrgExistByFilterAsync(o => o.SecondOrgName == command.SecondOrgName, cancellationToken))
            throw new BadRequestException("سازمانی با این نام وجود دارد، لطفا مقدار نام ثانویه را ویرایش کنید!");

        org.UpdateOrg(command.OrgName, command.SecondOrgName, command.OrgDescription);
        await _unitOfWork.SaveAsync(cancellationToken);

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> SoftDeleteOrganizationAsync(DeleteOrgAppDto command, CancellationToken cancellationToken)
    {
        // This method is used in transaction (TransAction)

        var org = await _organizationRepository.GetOrgByIdAsync(command.OrgId, true, cancellationToken);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.UserId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید آن را حذف کنید!");

        await _organizationRepository.LoadReferenceAsync(org, o => o.Owner, cancellationToken);

        if (!_commonService.Password.Verify(org.Owner.PasswordHash, command.UserPassword))
            throw new BadRequestException("رمز عبور اشتباه است!");

        org.SoftDelete();
        org.ChangeOrgActivity(false);
        
        // remove User From Org (Event)
        // delete Orgs Projects (Event)
        // delete Orgs Tasks (Event)

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> ChangeOrganizationActivityAsync(ChangeActivityOrgAppDto command, CancellationToken cancellationToken)
    {
        // This method is used in transaction (TransAction)

        var org = await _organizationRepository.GetOrgByIdAsync(command.OrgId, true, cancellationToken);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.UserId)
            throw new BadRequestException("شما مالک این سازمان نیستید و نمیتوانید آن را ویرایش کنید!");

        org.ChangeOrgActivity(command.Activity);

        // Inactive Orgs Projets (Event)
        // Inactive Orgs Tasks (Event)

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> AddUserToOrganizationAsync(AddUserOrgAppDto command, CancellationToken cancellationToken)
    {
        var org = await _organizationRepository.GetOrgByIdAsync(command.OrgId, false, cancellationToken);
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
    public async Task<GeneralResult> RemoveUserFromOrganizationAsync()
    {
        throw new NotImplementedException();
    }

    private async System.Threading.Tasks.Task CreateOrgMemberShipAsync(int orgId, int ownerId, OrganizationRoles role, CancellationToken cancellationToken)
    {
        var orgMemberShip = new OrganizationMemberShip(orgId, ownerId, role);

        await _unitOfWork.OrganizationMemberShipRepository.AddOrgMembershipAsync(orgMemberShip, cancellationToken);
    }
}
