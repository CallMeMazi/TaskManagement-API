using MediatR;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Query.GetOrgById;
public class GetOrgByIdHandler
    : IRequestHandler<GetOrgByIdQuery, GeneralResult<OrgDetailsDto>>
{
    private readonly IOrganizationService _organizationService;

    public GetOrgByIdHandler(IOrganizationService organizationService)
        => _organizationService = organizationService;

    public Task<GeneralResult<OrgDetailsDto>> Handle(GetOrgByIdQuery request, CancellationToken ct)
        => _organizationService.GetOrgByIdAsync(request.OrgId, ct);
}
