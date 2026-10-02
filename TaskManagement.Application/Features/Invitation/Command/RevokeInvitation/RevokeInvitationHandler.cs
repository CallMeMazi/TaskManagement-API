using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Invitation.Command.RevokeInvitation;
public class RevokeInvitationHandler
    : IRequestHandler<RevokeInvitationCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IInvitationService _invitationService;
    private readonly ICommonService _common;

    public RevokeInvitationHandler(IInvitationService invitationService, ICommonService common, IUnitOfWork uow)
    {
        _invitationService = invitationService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(RevokeInvitationCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<RevokeOrgInvitationAppDto>(request);

        await _invitationService.RevokeInvitationAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
