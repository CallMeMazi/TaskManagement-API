using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.ChangeOrgActivity;
public class ChangeOrgActivityHandler
    : IRequestHandler<ChangeOrgActivityCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public ChangeOrgActivityHandler(IOrganizationService organizationService, IMapper mapper, IUnitOfWork uow)
    {
        _organizationService = organizationService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeOrgActivityCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeActivityOrgAppDto>(request);

        var changeActivityRes = await _organizationService.ChangeOrgActivityAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return changeActivityRes;
    }
}
