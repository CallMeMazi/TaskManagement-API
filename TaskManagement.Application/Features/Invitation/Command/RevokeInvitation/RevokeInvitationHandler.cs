using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.Invitatoin;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Command.RevokeInvitation;
public class RevokeInvitationHandler
    : IRequestHandler<RevokeInvitationCommand, GeneralResult>
{
    private readonly IInvitationService _invitationService;
    private readonly IMapper _mapper;

    public RevokeInvitationHandler(IInvitationService invitationService, IMapper mapper)
    {
        _invitationService = invitationService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(RevokeInvitationCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<RevokeOrgInvitationAppDto>(request);

        return _invitationService.RevokeInvitationAsync(dto, ct);
    }
}
