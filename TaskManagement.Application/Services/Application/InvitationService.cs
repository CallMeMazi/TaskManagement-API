using TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Application.Utilities.Exceptions;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Enums.Statuses;
using TaskManagement.Domain.Interface.Services;

namespace TaskManagement.Application.Services.Application;
public class InvitationService : IInvitationService
{
    private readonly IUnitOfWork _uow;
    private readonly IInvitationDomainService _invitationDomainService;
    private readonly ICommonService _common;

    public InvitationService(IUnitOfWork uow, IInvitationDomainService invitationDomainService, ICommonService common)
    {
        _uow = uow;
        _invitationDomainService = invitationDomainService;
        _common = common;
    }

    // Query methods
    public async Task<OrgInvitationDetailsDto> GetOrgInvitationByIdAsync(long id, CancellationToken ct)
    {
        var invitation = await _uow.Invitation.GetByIdAsync(id, false, ct);
        if (invitation.IsNullParameter())
            throw new NotFoundException("درخواست دعوتی با این آیدی وجود ندارد!");

        var invitationDto = _common.Mapper.Map<OrgInvitationDetailsDto>(invitation);

        return invitationDto;
    }
    public async Task<OrgInvitationDetailsDto> GetPendingOrgInvitationByIdAsync(long id, CancellationToken ct)
    {
        var invitation = await _uow.Invitation.GetByFilterAsync(oi =>
            oi.Id == id
            && oi.Status == OrgInvitationStatusType.Pending,
            false,
            ct
        );
        if (invitation.IsNullParameter())
            throw new NotFoundException("درخواست دعوت فعالی با این آیدی وجود ندارد!");

        var invitationDto = _common.Mapper.Map<OrgInvitationDetailsDto>(invitation);

        return invitationDto;
    }
    public async Task<List<OrgInvitationDetailsDto>> GetAllOrgInvitationByOrgIdAsync(long orgId, CancellationToken ct)
    {
        var invitations = await _uow.Invitation.GetAllByFilterAsync(oi => oi.OrgId == orgId, false, ct);
        if (invitations.IsNullParameter() || !invitations.Any())
            throw new NotFoundException("درخواست دعوتی با این آیدی سازمان وجود ندارد!");

        var invitationsDto = _common.Mapper.Map<List<OrgInvitationDetailsDto>>(invitations);

        return invitationsDto;
    }
    public async Task<List<OrgInvitationDetailsDto>> GetAllPendingOrgInvitationByOrgIdAsync(long orgId, CancellationToken ct)
    {
        var invitations = await _uow.Invitation.GetAllByFilterAsync(oi =>
            oi.OrgId == orgId
            && oi.Status == OrgInvitationStatusType.Pending,
            false,
            ct
        );
        if (invitations.IsNullParameter() || !invitations.Any())
            throw new NotFoundException("درخواست دعوت فعالی با این آیدی سازمان وجود ندارد!");

        var invitationsDto = _common.Mapper.Map<List<OrgInvitationDetailsDto>>(invitations);

        return invitationsDto;
    }

    // Command methods
    public async Task<string> GenerateInviteLinkByUserIdAsync(CreateOrgInvitatoinAppDto command, CancellationToken ct)
    {
        var user = await _uow.User.GetByFilterAsync(u => u.MobileNumber == command.UserMobileNumber, false, ct);
        if (user.IsNullParameter())
            throw new NotFoundException("شماره موبایل کاربر نامعتبر است!");

        await _invitationDomainService.EnsureCanGenerateInviteLinkAsync(command.OrgId, command.OrgOwnerId, user!.Id, ct);

        var invatation = new OrganizationInvitation(command.OrgId, user!.Id);

        await _uow.Invitation.AddAsync(invatation, ct);

        return invatation.Token;
    }
    public async Task<long> AcceptInvitationAsync(AcceptOrgInvitationAppDto command, CancellationToken ct)
    {
        if (!await _uow.User.IsEntityExistByFilterAsync(u => u.Id == command.UserId, ct))
            throw new Exception($"user by {command.UserId} ID was not found. in {nameof(AcceptInvitationAsync)} method!");

        var invitation = await _uow.Invitation.GetByFilterAsync(oi =>
            oi.Token == command.Token
            && oi.UserId == command.UserId
            && oi.Status == OrgInvitationStatusType.Pending
            && oi.ExpiredAt > DateTime.Now,
            true,
            ct
        );
        if (invitation.IsNullParameter())
            throw new ValidationFailureException("لینک نامعتبر است!");

        invitation!.AcceptInvite();

        return invitation.OrgId;
    }
    public async System.Threading.Tasks.Task RevokeInvitationAsync(RevokeOrgInvitationAppDto command, CancellationToken ct)
    {
        var invitation = await _uow.Invitation.GetByFilterWithOrgAsync(oi =>
            oi.Id == command.InvitationId
            && oi.Status == OrgInvitationStatusType.Pending,
            true,
            ct
        );
        if (invitation.IsNullParameter())
            throw new NotFoundException("درخواست دعوت فعالی با این شناسه پیدا نشد!");

        if (invitation!.Org.OwnerId != command.OrgOwnerId)
            throw new ForbiddenException("شما مالک این سازمان نیستید!");

        invitation.RevokedInvite();
    }
}
