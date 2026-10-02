using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Organization.Command.UpdateOrg;
public class UpdateOrgHandler
    : IRequestHandler<UpdateOrgCommand>
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

    public async System.Threading.Tasks.Task Handle(UpdateOrgCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<UpdateOrgAppDto>(request);

        await _organizationService.UpdateOrgAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
