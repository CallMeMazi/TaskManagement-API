using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Command.AcceptInvitation;
public class AcceptInvitationHandler
    : IRequestHandler<AcceptInvitationCommand, GeneralResult>
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

    public async Task<GeneralResult> Handle(AcceptInvitationCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<AcceptOrgInvitationAppDto>(request);

        // Accept request and return orgid
        var accesptRes = await _invitationService.AcceptInvitationAsync(dto, ct);

        var addUserOrgRes = await _organizationService.AddUserToOrgAsync(new AddUserOrgAppDto(request.UserId, accesptRes.Result), ct);

        await _uow.SaveAsync(ct);

        return addUserOrgRes;
    }
}
