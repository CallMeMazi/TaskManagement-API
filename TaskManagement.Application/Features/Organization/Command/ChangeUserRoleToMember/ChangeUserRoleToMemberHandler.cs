using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.ChangeUserRoleToMember;
public class ChangeUserRoleToMemberHandler
    : IRequestHandler<ChangeUserRoleToMemberCommand, GeneralResult>
{
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public ChangeUserRoleToMemberHandler(IOrganizationService organizationService, IMapper mapper)
    {
        _organizationService = organizationService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChangeUserRoleToMemberCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeUserRoleOrgAppDto>(request);

        return _organizationService.ChangeUserRoleToMemberAsync(dto, ct);
    }
}
