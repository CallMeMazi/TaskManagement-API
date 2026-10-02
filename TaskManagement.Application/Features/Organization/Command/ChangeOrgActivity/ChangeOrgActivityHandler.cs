using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Organization.Command.ChangeOrgActivity;
public class ChangeOrgActivityHandler
    : IRequestHandler<ChangeOrgActivityCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly ICommonService _common;

    public ChangeOrgActivityHandler(IOrganizationService organizationService, ICommonService common, IUnitOfWork uow)
    {
        _organizationService = organizationService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(ChangeOrgActivityCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeActivityOrgAppDto>(request);

        await _organizationService.ChangeOrgActivityAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
