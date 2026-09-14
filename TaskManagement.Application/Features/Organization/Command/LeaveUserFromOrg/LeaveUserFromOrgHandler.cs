using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.LeaveUserFromOrg;
public class LeaveUserFromOrgHandler
    : IRequestHandler<LeaveUserFromOrgCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public LeaveUserFromOrgHandler(IOrganizationService organizationService, IMapper mapper, IUnitOfWork uow)
    {
        _organizationService = organizationService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(LeaveUserFromOrgCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<LeaveUserOrgAppDto>(request);

        var leaveUserOrg = await _organizationService.LeaveUserFromOrgAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return leaveUserOrg;
    }
}
