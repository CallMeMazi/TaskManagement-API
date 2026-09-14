using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.RemoveUserFromOrg;
public class RemoveUserFromOrgHandler
    : IRequestHandler<RemoveUserFromOrgCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public RemoveUserFromOrgHandler(IOrganizationService organizationService, IMapper mapper, IUnitOfWork uow)
    {
        _organizationService = organizationService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(RemoveUserFromOrgCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<RemoveUserOrgAppDto>(request);

        var removeUserOrg = await _organizationService.RemoveUserFromOrgAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return removeUserOrg;
    }
}
