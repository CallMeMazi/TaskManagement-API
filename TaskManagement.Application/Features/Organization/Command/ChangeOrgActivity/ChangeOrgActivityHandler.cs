using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.ChangeOrgActivity;
public class ChangeOrgActivityHandler
    : IRequestHandler<ChangeOrgActivityCommand, GeneralResult>
{
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public ChangeOrgActivityHandler(IOrganizationService organizationService, IMapper mapper)
    {
        _organizationService = organizationService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChangeOrgActivityCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeActivityOrgAppDto>(request);

        return _organizationService.ChangeOrgActivityAsync(dto, ct);
    }
}
