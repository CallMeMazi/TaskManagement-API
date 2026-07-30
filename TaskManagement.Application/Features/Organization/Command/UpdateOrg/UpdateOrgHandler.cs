using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.UpdateOrg;
public class UpdateOrgHandler
    : IRequestHandler<UpdateOrgCommand, GeneralResult>
{
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public UpdateOrgHandler(IOrganizationService organizationService, IMapper mapper)
    {
        _organizationService = organizationService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(UpdateOrgCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UpdateOrgAppDto>(request);

        return _organizationService.UpdateOrgAsync(dto, ct);
    }
}
