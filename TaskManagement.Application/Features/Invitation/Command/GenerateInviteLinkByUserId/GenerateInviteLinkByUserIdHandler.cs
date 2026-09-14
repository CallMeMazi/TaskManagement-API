using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Command.GenerateInviteLinkByUserId;
public class GenerateInviteLinkByUserIdHandler
    : IRequestHandler<GenerateInviteLinkByUserIdCommand, GeneralResult<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IInvitationService _invitationService;
    private readonly IMapper _mapper;

    public GenerateInviteLinkByUserIdHandler(IInvitationService invitationService, IMapper mapper, IUnitOfWork uow)
    {
        _invitationService = invitationService;
        _mapper = mapper;
        _uow = uow;
    }
    public async Task<GeneralResult<string>> Handle(GenerateInviteLinkByUserIdCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<CreateOrgInvitatoinAppDto>(request);

        var inviteUserRes = await _invitationService.GenerateInviteLinkByUserIdAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return inviteUserRes;
    }
}
