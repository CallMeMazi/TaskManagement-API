using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToMember;
internal class ChangeUserRoleToMemberHandler
    : IRequestHandler<ChangeUserRoleToMemberCommand, GeneralResult>
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ChangeUserRoleToMemberHandler(IProjectService projectService, IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChangeUserRoleToMemberCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeUserRoleProjectAppDto>(request);

        return _projectService.ChangeUserRoleToMemberAsync(dto, ct);
    }
}
