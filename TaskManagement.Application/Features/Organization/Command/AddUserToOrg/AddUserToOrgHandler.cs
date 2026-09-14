using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.AddUserToOrg;
public class AddUserToOrgHandler
    : IRequestHandler<AddUserToOrgCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public AddUserToOrgHandler(IOrganizationService organizationService, IMapper mapper, IUnitOfWork uow)
    {
        _organizationService = organizationService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(AddUserToOrgCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<AddUserOrgAppDto>(request);

        var addUserOrgRes = await _organizationService.AddUserToOrgAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return addUserOrgRes;
    }
}
