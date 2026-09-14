using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Command.RevokeInvitation;
public class RevokeInvitationHandler
    : IRequestHandler<RevokeInvitationCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IInvitationService _invitationService;
    private readonly IMapper _mapper;

    public RevokeInvitationHandler(IInvitationService invitationService, IMapper mapper, IUnitOfWork uow)
    {
        _invitationService = invitationService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(RevokeInvitationCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<RevokeOrgInvitationAppDto>(request);

        var revokeInvateRes = await _invitationService.RevokeInvitationAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return revokeInvateRes;
    }
}
