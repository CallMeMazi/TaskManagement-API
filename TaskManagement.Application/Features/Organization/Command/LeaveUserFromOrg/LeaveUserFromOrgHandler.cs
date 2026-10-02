using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.LeaveUserFromOrg;
public class LeaveUserFromOrgHandler
    : IRequestHandler<LeaveUserFromOrgCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly ICommonService _common;

    public LeaveUserFromOrgHandler(IOrganizationService organizationService, ICommonService common, IUnitOfWork uow)
    {
        _organizationService = organizationService;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(LeaveUserFromOrgCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<LeaveUserOrgAppDto>(request);

        var leaveUserOrg = await _organizationService.LeaveUserFromOrgAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return leaveUserOrg;
    }
}
