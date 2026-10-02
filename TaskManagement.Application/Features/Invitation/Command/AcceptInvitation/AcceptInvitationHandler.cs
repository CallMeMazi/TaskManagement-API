using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Invitation.Command.AcceptInvitation;
public class AcceptInvitationHandler
    : IRequestHandler<AcceptInvitationCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IInvitationService _invitationService;
    private readonly IOrganizationService _organizationService;
    private readonly ICommonService _common;

    public AcceptInvitationHandler(IInvitationService invitationService, IOrganizationService organizationService, ICommonService common
        , IUnitOfWork uow)
    {
        _invitationService = invitationService;
        _organizationService = organizationService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(AcceptInvitationCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<AcceptOrgInvitationAppDto>(request);

        // Accept request and return orgid
        var orgId = await _invitationService.AcceptInvitationAsync(dto, ct);

        await _organizationService.AddUserToOrgAsync(new AddUserOrgAppDto(request.UserId, orgId), ct);

        await _uow.SaveAsync(ct);
    }
}
