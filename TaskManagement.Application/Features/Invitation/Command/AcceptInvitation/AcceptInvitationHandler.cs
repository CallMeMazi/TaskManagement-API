using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.Invitatoin;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Command.AcceptInvitation;
public class AcceptInvitationHandler
    : IRequestHandler<AcceptInvitationCommand, GeneralResult>
{
    private readonly IInvitationService _invitationService;
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public AcceptInvitationHandler(IInvitationService invitationService, IOrganizationService organizationService, IMapper mapper)
    {
        _invitationService = invitationService;
        _organizationService = organizationService;
        _mapper = mapper;
    }

    public async Task<GeneralResult> Handle(AcceptInvitationCommand request, CancellationToken ct)
    {
        // TransActional

        var dto = _mapper.Map<AcceptOrgInvitationAppDto>(request);

        var accesptRes = await _invitationService.AcceptInvitationAsync(dto, ct);

        return await _organizationService.AddUserToOrgAsync(new AddUserOrgAppDto(request.UserId, accesptRes.Result), ct);
    }
}
