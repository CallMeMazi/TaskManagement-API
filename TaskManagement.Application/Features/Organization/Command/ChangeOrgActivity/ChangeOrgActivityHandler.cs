using MediatR;
using TaskManagement.Application.Auditing;
using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Organization.Command.ChangeOrgActivity;
public class ChangeOrgActivityHandler
    : IRequestHandler<ChangeOrgActivityCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly ICommonService _common;
    private readonly IAuditLogService _audit;

    public ChangeOrgActivityHandler(
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

    public async System.Threading.Tasks.Task Handle(ChangeOrgActivityCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeActivityOrgAppDto>(request);

        await _organizationService.ChangeOrgActivityAsync(dto, ct);

        _audit.Stage(new AuditEntryInternalDto(
            request.Activity ? AuditActions.OrganizationActivated : AuditActions.OrganizationDeactivated,
            EntityType.Organization,
            request.OrgId,
            request.OwnerId,
            Metadata: AuditMetadata.Pairs(("IsActive", request.Activity ? "true" : "false"))));

        await _uow.SaveAsync(ct);
    }
}
