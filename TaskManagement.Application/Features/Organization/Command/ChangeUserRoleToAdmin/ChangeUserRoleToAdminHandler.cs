using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.ChangeUserRoleToAdmin;
public class ChangeUserRoleToAdminHandler
    : IRequestHandler<ChangeUserRoleToAdminCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly ICommonService _common;

    public ChangeUserRoleToAdminHandler(IOrganizationService organizationService, ICommonService common, IUnitOfWork uow)
    {
        _organizationService = organizationService;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeUserRoleToAdminCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeUserRoleOrgAppDto>(request);

        var changeUserRole = await _organizationService.ChangeUserRoleToAdminAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return changeUserRole;
    }
}
