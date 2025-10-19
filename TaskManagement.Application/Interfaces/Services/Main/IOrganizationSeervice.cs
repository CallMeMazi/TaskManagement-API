using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Main;
public interface IOrganizationSeervice
{
    Task<GeneralResult<OrgDetailsDto>> GetOrganizationByIdCodeAsync(string orgName, CancellationToken cancellationToken);
    Task<GeneralResult> CreateOrganizationAsync(CreateOrgAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> UpdateOrganizationAsync(UpdateOrgAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> SoftDeleteOrganizationAsync(DeleteOrgAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> ChangeOrganizationActivityAsync(ChangeActivityOrgAppDto command, CancellationToken cancellationToken);
}
