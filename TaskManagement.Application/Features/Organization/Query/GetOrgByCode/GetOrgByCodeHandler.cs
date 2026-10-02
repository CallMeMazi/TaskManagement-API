using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;

namespace TaskManagement.Application.Features.Organization.Query.GetOrgByCode;
public class GetOrgByCodeHandler
    : IRequestHandler<GetOrgByCodeQuery, OrgDetailsDto>
{
    private readonly IOrganizationService _organizationService;

    public GetOrgByCodeHandler(IOrganizationService organizationService)
        => _organizationService = organizationService;

    public Task<OrgDetailsDto> Handle(GetOrgByCodeQuery request, CancellationToken ct)
        => _organizationService.GetOrgByCodeAsync(request.OrgCode, ct);
}
