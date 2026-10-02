using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.DeleteOrg;
public class DeleteOrgHandler
    : IRequestHandler<DeleteOrgCommand, GeneralResult>
{
    private readonly IOrganizationService _organizationService;
    private readonly ICommonService _common;

    public DeleteOrgHandler(IOrganizationService organizationService, ICommonService common)
    {
        _organizationService = organizationService;
        _common = common;
    }
    public Task<GeneralResult> Handle(DeleteOrgCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<DeleteOrgAppDto>(request);

        return _organizationService.SoftDeleteOrgAsync(dto, ct);
    }
}
