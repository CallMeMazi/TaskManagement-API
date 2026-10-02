using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;

namespace TaskManagement.Application.Features.Organization.Query.GetOrgById;
public class GetOrgByIdHandler
    : IRequestHandler<GetOrgByIdQuery, OrgDetailsDto>
{
    private readonly IOrganizationService _organizationService;

    public GetOrgByIdHandler(IOrganizationService organizationService)
        => _organizationService = organizationService;

    public Task<OrgDetailsDto> Handle(GetOrgByIdQuery request, CancellationToken ct)
        => _organizationService.GetOrgByIdAsync(request.OrgId, ct);
}
