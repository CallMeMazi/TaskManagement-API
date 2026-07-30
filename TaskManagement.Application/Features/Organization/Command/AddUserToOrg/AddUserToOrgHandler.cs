using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.AddUserToOrg;
public class AddUserToOrgHandler
    : IRequestHandler<AddUserToOrgCommand, GeneralResult>
{
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public AddUserToOrgHandler(IOrganizationService organizationService, IMapper mapper)
    {
        _organizationService = organizationService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(AddUserToOrgCommand request, CancellationToken ct)
    {
        // TransActional

        var dto = _mapper.Map<AddUserOrgAppDto>(request);

        return _organizationService.AddUserToOrgAsync(dto, ct);
    }
}
