using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.UpdateOrg;
public class UpdateOrgHandler
    : IRequestHandler<UpdateOrgCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly ICommonService _common;

    public UpdateOrgHandler(IOrganizationService organizationService, ICommonService common, IUnitOfWork uow)
    {
        _organizationService = organizationService;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(UpdateOrgCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<UpdateOrgAppDto>(request);

        var updateOrgRes = await _organizationService.UpdateOrgAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return updateOrgRes;
    }
}
