using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Organization.Command.CreateOrg;
public class CreateOrgHandler
    : IRequestHandler<CreateOrgCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly ICommonService _common;

    public CreateOrgHandler(IOrganizationService organizationService, ICommonService common, IUnitOfWork uow)
    {
        _organizationService = organizationService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(CreateOrgCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<CreateOrgAppDto>(request);

        await _organizationService.CreateOrgAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
