using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Invitatoin;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.DTOs.SharedDTOs.Invitation;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Application.Services.Application;
public class InvitationService : IInvitationService
{
    private readonly IUnitOfWork _uow;
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;


    public InvitationService(IUnitOfWork uow, IEventService eventService, IMapper mapper)
    {
        _uow = uow;
        _eventService = eventService;
        _mapper = mapper;
    }


    // Query methods
    public async Task<GeneralResult<OrgInvitationDetailsDto>> GetOrgInvitationByIdAsync(int id, CancellationToken ct)
    {
        var invitation = await _uow.Invitation.GetByIdAsync(id, false, ct);

        if (invitation.IsNullParameter())
            throw new NotFoundException("درخواست دعوتی با این آیدی وجود ندارد!");

        var invitationDto = _mapper.Map<OrgInvitationDetailsDto>(invitation);

        return GeneralResult<OrgInvitationDetailsDto>.Success(invitationDto);
    }
    public async Task<GeneralResult<OrgInvitationDetailsDto>> GetPendingOrgInvitationByIdAsync(int id, CancellationToken ct)
    {
        var invitation = await _uow.Invitation.GetByFilterAsync(oi =>
            oi.Id == id
            && oi.Status == OrgInvitationStatus.Pending,
            false,
            ct
        );

        if (invitation.IsNullParameter())
            throw new NotFoundException("درخواست دعوت فعالی با این آیدی وجود ندارد!");

        var invitationDto = _mapper.Map<OrgInvitationDetailsDto>(invitation);

        return GeneralResult<OrgInvitationDetailsDto>.Success(invitationDto);
    }
    public async Task<GeneralResult<List<OrgInvitationDetailsDto>>> GetAllOrgInvitationByOrgIdAsync(int orgId, CancellationToken ct)
    {
        var invitations = await _uow.Invitation.GetAllByFilterAsync(oi => oi.OrgId == orgId, false, ct);

        if (invitations.IsNullParameter() || !invitations.Any())
            throw new NotFoundException("درخواست دعوتی با این آیدی سازمان وجود ندارد!");

        var invitationsDto = _mapper.Map<List<OrgInvitationDetailsDto>>(invitations);

        return GeneralResult<List<OrgInvitationDetailsDto>>.Success(invitationsDto);
    }
    public async Task<GeneralResult<List<OrgInvitationDetailsDto>>> GetAllPendingOrgInvitationByOrgIdAsync(int orgId, CancellationToken ct)
    {
        var invitations = await _uow.Invitation.GetAllByFilterAsync(oi =>
            oi.OrgId == orgId
            && oi.Status == OrgInvitationStatus.Pending,
            false,
            ct
        );

        if (invitations.IsNullParameter() || !invitations.Any())
            throw new NotFoundException("درخواست دعوت فعالی با این آیدی سازمان وجود ندارد!");

        var invitationsDto = _mapper.Map<List<OrgInvitationDetailsDto>>(invitations);

        return GeneralResult<List<OrgInvitationDetailsDto>>.Success(invitationsDto);
    }

    // Command methods
    public async Task<GeneralResult<string>> GenerateInviteLinkByUserIdAsync(CreateOrgInvitatoinAppDto command, CancellationToken ct)
    {
        var user = await _uow.User.GetByFilterAsync(u => u.MobileNumber == command.UserMobileNumber, false, ct);
        if (user.IsNullParameter())
            throw new NotFoundException("شماره موبایل کاربر نامعتبر است!");

        var org = await _uow.Organization.GetByIdAsync(command.OrgId, false, ct);
        if (org.IsNullParameter())
            throw new NotFoundException("شناسه سازمان نامعتبر است!");

        if (org!.OwnerId != command.OrgOwnerId)
            throw new BadRequestException("فقط مالک سازمان میتواند لینک دعوت بسازد!");

        var isInvitationExist = await _uow.Invitation.IsEntityExistByFilterAsync(oi =>
            oi.UserId == user!.Id
            && oi.OrgId == org.Id
            && oi.Status == OrgInvitationStatus.Pending,
            ct
        );
        if (isInvitationExist)
            throw new ForbiddenException("شما برای این کاربر لینک دعوت فعال دارید!");

        var isUserInOrg = await _uow.OrganizationMemberShip.IsEntityExistByFilterAsync(om =>
            om.OrgId == org.Id
            & om.UserId == user!.Id,
            ct
        );
        if (isUserInOrg)
            throw new BadRequestException("این کاربر در حال حاضر در سازمان وجود دارد!");

        var invatation = new OrganizationInvitation(org.Id, user!.Id);

        await _uow.Invitation.AddAsync(invatation, ct);
        await _uow.SaveAsync(ct);

        return GeneralResult<string>.Success(invatation.Token);
    }
    public async Task<GeneralResult> AcceptInvitationAsync(AcceptOrgInvitationAppDto command, CancellationToken ct)
    {
        // This method is used in transaction (TransAction)

        if (!await _uow.User.IsEntityExistByFilterAsync(u => u.Id == command.UserId, ct))
            throw new Exception($"user by {command.UserId} ID was not found. in {nameof(AcceptInvitationAsync)} method!");

        var invitation = await _uow.Invitation.GetByFilterAsync(oi =>
            oi.Token == command.Token
            && oi.UserId == command.UserId
            && oi.Status == OrgInvitationStatus.Pending
            && oi.ExpiredAt > DateTime.Now,
            true,
            ct
        );
        if (invitation.IsNullParameter())
            throw new BadRequestException("لینک نامعتبر است!");

        invitation!.AcceptInvite();
        await _uow.SaveAsync(ct);

        // Create Relation between User And Org (Event)
        await _eventService.PublishAddUserToOrgEventAsync(
            new AddUserOrgAppDto() { OrgId = invitation.OrgId, UserId = command.UserId },
            ct
        );

        return GeneralResult.Success();
    }
    public async Task<GeneralResult> RevokeInvitationAsync(RevokeOrgInvitationAppDto command, CancellationToken ct)
    {
        var invitation = await _uow.Invitation.GetByFilterWithOrgAsync(oi =>
            oi.Id == command.InvitationId
            && oi.Status == OrgInvitationStatus.Pending,
            true,
            ct
        );
        if (invitation.IsNullParameter())
            throw new NotFoundException("درخواست دعوت فعالی با این شناسه پیدا نشد!");

        if (invitation!.Org.OwnerId != command.OrgOwnerId)
            throw new ForbiddenException("شما مالک این سازمان نیستید!");

        invitation.RevokedInvite();
        await _uow.SaveAsync(ct);

        return GeneralResult.Success();
    }
}
