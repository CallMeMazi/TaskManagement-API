using TaskManagement.Domain.Enums.Statuses;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Domain.Interface.Services;
using TaskManagement.Domain.Utilities.Exceptions;

namespace TaskManagement.Domain.Services;

public class OrganizationDomainService : IOrganizationDomainService
{
    private readonly IOrganizationRepository _orgRepository;
    private readonly IOrganizationMemberShipRepository _orgMemberShipRepository;
    private readonly IProjectRepository _projectRepository;


    public OrganizationDomainService(IOrganizationRepository orgRepository, IProjectRepository projectRepository, IOrganizationMemberShipRepository orgMemberShipRepository)
    {
        _orgRepository = orgRepository;
        _projectRepository = projectRepository;
        _orgMemberShipRepository = orgMemberShipRepository;
    }


    public async Task EnsureCanCreateOrgAsync(string secondOrgName, long ownerId, CancellationToken ct)
    {
        if (await _orgRepository.IsEntityExistByFilterAsync(o => o.SecondOrgName == secondOrgName, ct))
            throw new DomainLogicalException("سازمانی با این نام وجود دارد، لطفا مقدار نام ثانویه را ویرایش کنید!");

        if (await _orgRepository.IsEntityExistByFilterAsync(o => o.OwnerId == ownerId && o.IsActive, ct))
            throw new DomainLogicalException("شما نمیتوانید چندین سازمان فعال داشته باشید، لطفا ابتدا سازمان فعلی خود را غیرفعال کیند");

        var userOrgCount = await _orgRepository.GetCountByFilterAsync(o => o.OwnerId == ownerId, ct);
        if (userOrgCount == 3)
            throw new DomainLogicalException("نمیوتنید بیشتر از 3 سازمان به نام خود داشته باشید!");
    }
    public async Task EnsureCanUpdateOrgAsync(string secondOrgName, long orgId, CancellationToken ct)
    {
        if (await _orgRepository.IsEntityExistByFilterAsync(o => o.SecondOrgName == secondOrgName && o.Id != orgId, ct))
            throw new DomainLogicalException("سازمانی با این نام وجود دارد، لطفا مقدار نام ثانویه را ویرایش کنید!");
    }
    public async Task EnsureCanDeactiveOrgAsync(long orgId, CancellationToken ct)
    {
        var isProjActive = await _projectRepository.IsEntityExistByFilterAsync(p =>
            p.OrgId == orgId
            && (p.ProjStatus == ProjectStatusType.InProgress || p.ProjStatus == ProjectStatusType.Adjournment),
            ct
        );
        if (isProjActive)
            throw new DomainLogicalException("در سازمان شما پروژه فعال وجود دارد، اول آن را کنسل یا به اتمام برسانید!");
    }
    // Org Membership methods
    public async Task EnsureCanUserAddToOrgAsync(long orgId, long userId, CancellationToken ct)
    {
        var isUserInOrg = await _orgMemberShipRepository.IsEntityExistByFilterAsync(o =>
            o.OrgId == orgId
            && o.UserId == userId,
            ct
        );
        if (isUserInOrg)
            throw new DomainLogicalException("شما در این سازمان حضور دارید!");
    }
    public async Task EnsureCanRemoveUserFromOrgAsync(long orgId, long userId, CancellationToken ct)
    {
        var isUserInActiveProj = await _projectRepository.IsEntityExistByFilterAsync(p =>
            p.OrgId == orgId
            && (p.ProjStatus == ProjectStatusType.InProgress || p.ProjStatus == ProjectStatusType.Adjournment)
            && p.ProjMember.Any(pm => pm.UserId == userId),
            ct
        );
        if (isUserInActiveProj)
            throw new DomainLogicalException("کاربر مورد نظر در پروژه فعال حضور دارد، ابتدا پروژه را به اتمام برسانید یا کنسل کنید یا کاربر را از پروژه حذف کنید!");
    }
    public async Task EnsureCanChangeRoleToMemberAsync(long userId, long orgId, CancellationToken ct)
    {
        var isUserHasActiveProj = await _projectRepository.IsEntityExistByFilterAsync(p =>
            p.CreatorId == userId
            && p.OrgId == orgId
            && (p.ProjStatus == ProjectStatusType.InProgress || p.ProjStatus == ProjectStatusType.Adjournment),
            ct
        );
        if (isUserHasActiveProj)
            throw new DomainLogicalException("این ادمین دارای پروژه های فعال است!");
    }
}
