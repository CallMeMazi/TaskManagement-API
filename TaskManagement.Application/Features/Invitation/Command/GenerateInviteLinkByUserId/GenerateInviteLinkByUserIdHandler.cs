using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Command.GenerateInviteLinkByUserId;
public class GenerateInviteLinkByUserIdHandler
    : IRequestHandler<GenerateInviteLinkByUserIdCommand, GeneralResult<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IInvitationService _invitationService;
    private readonly ICommonService _common;

    public GenerateInviteLinkByUserIdHandler(IInvitationService invitationService, ICommonService common, IUnitOfWork uow)
    {
        _invitationService = invitationService;
        _common = common;
        _uow = uow;
    }
    public async Task<GeneralResult<string>> Handle(GenerateInviteLinkByUserIdCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<CreateOrgInvitatoinAppDto>(request);

        var inviteUserRes = await _invitationService.GenerateInviteLinkByUserIdAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return inviteUserRes;
    }
}
