using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Command.AcceptInvitation;
public class AcceptInvitationHandler
    : IRequestHandler<AcceptInvitationCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IInvitationService _invitationService;
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public AcceptInvitationHandler(IInvitationService invitationService, IOrganizationService organizationService, IMapper mapper, IUnitOfWork uow)
    {
        _invitationService = invitationService;
        _organizationService = organizationService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(AcceptInvitationCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<AcceptOrgInvitationAppDto>(request);

        // Accept request and return orgid
        var accesptRes = await _invitationService.AcceptInvitationAsync(dto, ct);

        var addUserOrgRes = await _organizationService.AddUserToOrgAsync(new AddUserOrgAppDto(request.UserId, accesptRes.Result), ct);

        await _uow.SaveAsync(ct);

        return addUserOrgRes;
    }
}
