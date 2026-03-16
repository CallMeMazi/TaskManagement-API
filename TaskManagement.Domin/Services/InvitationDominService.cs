using System.Net;
using TaskManagement.Common.Enums;
using TaskManagement.Common.Exceptions;
using TaskManagement.Domin.Enums.Statuses;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Domin.Interface.Services;

namespace TaskManagement.Domin.Services;
public class InvitationDominService : IInvitationDominService
{
    private readonly IOrganizationInvitationRepository _invitationRepository;
    private readonly IOrganizationRepository _orgRepository;
    private readonly IOrganizationMemberShipRepository _orgMembershipRepository;


    public InvitationDominService(IOrganizationRepository orgRepository, IOrganizationInvitationRepository invitationRepository
        , IOrganizationMemberShipRepository orgMembershipRepository)
    {
        _invitationRepository = invitationRepository;
        _orgRepository = orgRepository;
        _orgMembershipRepository = orgMembershipRepository;
    }


    public async Task EnsureCanGenerateInviteLinkAsync(int orgId, int orgOwnerId, int userId, CancellationToken ct)
    {
        var isUserOrgOwner = await _orgRepository.IsEntityExistByFilterAsync(o =>
            o.Id == orgId
            && o.OwnerId == orgOwnerId,
            ct
        );
        if (isUserOrgOwner)
            throw new ForbiddenException("فقط مالک سازمان میتواند لینک دعوت بسازد!");

        var isInvitationExist = await _invitationRepository.IsEntityExistByFilterAsync(oi =>
            oi.UserId == userId
            && oi.OrgId == orgId
            && oi.Status == OrgInvitationStatus.Pending,
            ct
        );
        if (isInvitationExist)
            throw new BadRequestException("شما برای این کاربر لینک دعوت فعال دارید!");

        var isUserInOrg = await _orgMembershipRepository.IsEntityExistByFilterAsync(om =>
            om.OrgId == orgId
            & om.UserId == userId,
            ct
        );
        if (isUserInOrg)
            throw new BadRequestException("این کاربر در حال حاضر در سازمان وجود دارد!");
    }
}
