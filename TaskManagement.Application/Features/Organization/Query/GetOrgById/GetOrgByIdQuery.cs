using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Organization;

namespace TaskManagement.Application.Features.Organization.Query.GetOrgById;
public record GetOrgByIdQuery(long OrgId)
    : IRequest<OrgDetailsDto>;