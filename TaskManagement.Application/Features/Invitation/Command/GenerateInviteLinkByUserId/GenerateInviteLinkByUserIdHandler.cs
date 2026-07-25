using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.Invitatoin;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Command.GenerateInviteLinkByUserId;
public class GenerateInviteLinkByUserIdHandler
    : IRequestHandler<GenerateInviteLinkByUserIdCommand, GeneralResult<string>>
{
    private readonly IInvitationService _invitationService;
    private readonly IMapper _mapper;

    public GenerateInviteLinkByUserIdHandler(IInvitationService invitationService, IMapper mapper)
    {
        _invitationService = invitationService;
        _mapper = mapper;
    }
    public Task<GeneralResult<string>> Handle(GenerateInviteLinkByUserIdCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<CreateOrgInvitatoinAppDto>(request);

        return _invitationService.GenerateInviteLinkByUserIdAsync(dto, ct);
    }
}
