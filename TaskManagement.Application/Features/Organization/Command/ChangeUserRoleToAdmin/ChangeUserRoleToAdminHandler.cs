using MediatR;
using TaskManagement.Application.Auditing;
using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Organization.Command.ChangeUserRoleToAdmin;
public class ChangeUserRoleToAdminHandler
    : IRequestHandler<ChangeUserRoleToAdminCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly ICommonService _common;
    private readonly IAuditLogService _audit;

    public ChangeUserRoleToAdminHandler(
        IOrganizationService organizationService,
        ICommonService common,
        IUnitOfWork uow,
        IAuditLogService audit)
    {
        _organizationService = organizationService;
        _common = common;
        _uow = uow;
        _audit = audit;
    }

    public async System.Threading.Tasks.Task Handle(ChangeUserRoleToAdminCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeUserRoleOrgAppDto>(request);

        await _organizationService.ChangeUserRoleToAdminAsync(dto, ct);

        _audit.Stage(new AuditEntryInternalDto(
            AuditActions.OrganizationRoleChanged,
            EntityType.Organization,
            request.OrgId,
            request.OrgOwnerId,
            EntityType.User,
            request.UserId,
            AuditMetadata.Pairs(("Role", "Admin"))));

        await _uow.SaveAsync(ct);
    }
}
