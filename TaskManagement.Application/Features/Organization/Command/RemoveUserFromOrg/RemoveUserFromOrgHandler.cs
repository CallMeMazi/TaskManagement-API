using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.RemoveUserFromOrg;
public class RemoveUserFromOrgHandler
    : IRequestHandler<RemoveUserFromOrgCommand, GeneralResult>
{
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public RemoveUserFromOrgHandler(IOrganizationService organizationService, IMapper mapper)
    {
        _organizationService = organizationService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(RemoveUserFromOrgCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<RemoveUserOrgAppDto>(request);

        return _organizationService.RemoveUserFromOrgAsync(dto, ct);
    }
}
