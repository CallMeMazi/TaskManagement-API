using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Organization;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Query.GetOrgByCode;
public record GetOrgByCodeQuery(string OrgCode)
    : IRequest<GeneralResult<OrgDetailsDto>>;