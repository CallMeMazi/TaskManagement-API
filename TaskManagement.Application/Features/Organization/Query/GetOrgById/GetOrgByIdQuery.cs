using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Organization;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Query.GetOrgById;
public record GetOrgByIdQuery(long OrgId)
    : IRequest<GeneralResult<OrgDetailsDto>>;