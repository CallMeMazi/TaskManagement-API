using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.DeleteOrg;
public class DeleteOrgHandler
    : IRequestHandler<DeleteOrgCommand, GeneralResult>
{
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public DeleteOrgHandler(IOrganizationService organizationService, IMapper mapper)
    {
        _organizationService = organizationService;
        _mapper = mapper;
    }
    public Task<GeneralResult> Handle(DeleteOrgCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<DeleteOrgAppDto>(request);

        return _organizationService.SoftDeleteOrgAsync(dto, ct);
    }
}
