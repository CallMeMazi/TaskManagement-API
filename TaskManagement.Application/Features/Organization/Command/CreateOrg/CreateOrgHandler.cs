using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.CreateOrg;
public class CreateOrgHandler
    : IRequestHandler<CreateOrgCommand, GeneralResult>
{
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public CreateOrgHandler(IOrganizationService organizationService, IMapper mapper)
    {
        _organizationService = organizationService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(CreateOrgCommand request, CancellationToken ct)
    {
        // TransActional

        var dto = _mapper.Map<CreateOrgAppDto>(request);

        return _organizationService.CreateOrgAsync(dto, ct);
    }
}
