using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Organization;

namespace TaskManagement.Application.Features.Organization.Query.GetOrgByCode;
public record GetOrgByCodeQuery(string OrgCode)
    : IRequest<OrgDetailsDto>;