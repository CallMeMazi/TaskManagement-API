using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.LeaveUserFromOrg;
public class LeaveUserFromOrgHandler
    : IRequestHandler<LeaveUserFromOrgCommand, GeneralResult>
{
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public LeaveUserFromOrgHandler(IOrganizationService organizationService, IMapper mapper)
    {
        _organizationService = organizationService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(LeaveUserFromOrgCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<LeaveUserOrgAppDto>(request);

        return _organizationService.LeaveUserFromOrgAsync(dto, ct);
    }
}
