using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.CreateOrg;
public class CreateOrgHandler
    : IRequestHandler<CreateOrgCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public CreateOrgHandler(IOrganizationService organizationService, IMapper mapper, IUnitOfWork uow)
    {
        _organizationService = organizationService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(CreateOrgCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<CreateOrgAppDto>(request);

        var createOrgRes = await _organizationService.CreateOrgAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return createOrgRes;
    }
}
