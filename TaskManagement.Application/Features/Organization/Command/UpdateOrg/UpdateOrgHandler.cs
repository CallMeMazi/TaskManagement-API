using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.UpdateOrg;
public class UpdateOrgHandler
    : IRequestHandler<UpdateOrgCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public UpdateOrgHandler(IOrganizationService organizationService, IMapper mapper, IUnitOfWork uow)
    {
        _organizationService = organizationService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(UpdateOrgCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UpdateOrgAppDto>(request);

        var updateOrgRes = await _organizationService.UpdateOrgAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return updateOrgRes;
    }
}
