using MediatR;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Query.GetOrgByCode;
public class GetOrgByCodeHandler
    : IRequestHandler<GetOrgByCodeQuery, GeneralResult<OrgDetailsDto>>
{
    private readonly IOrganizationService _organizationService;

    public GetOrgByCodeHandler(IOrganizationService organizationService)
        => _organizationService = organizationService;

    public Task<GeneralResult<OrgDetailsDto>> Handle(GetOrgByCodeQuery request, CancellationToken ct)
        => _organizationService.GetOrgByCodeAsync(request.OrgCode, ct);
}
