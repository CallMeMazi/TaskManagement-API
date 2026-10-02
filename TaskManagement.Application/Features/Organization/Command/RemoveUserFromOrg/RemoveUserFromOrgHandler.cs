using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.RemoveUserFromOrg;
public class RemoveUserFromOrgHandler
    : IRequestHandler<RemoveUserFromOrgCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly ICommonService _common;

    public RemoveUserFromOrgHandler(IOrganizationService organizationService, ICommonService common, IUnitOfWork uow)
    {
        _organizationService = organizationService;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(RemoveUserFromOrgCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<RemoveUserOrgAppDto>(request);

        var removeUserOrg = await _organizationService.RemoveUserFromOrgAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return removeUserOrg;
    }
}
