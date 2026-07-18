using MediatR;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Query.GetOrgById;
public record GetOrgByIdQuery(int OrgId)
    : IRequest<GeneralResult<OrgDetailsDto>>;