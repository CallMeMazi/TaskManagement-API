using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToAdmin;
public class ChangeUserRoleToAdminHandler
    : IRequestHandler<ChangeUserRoleToAdminCommand, GeneralResult>
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ChangeUserRoleToAdminHandler(IProjectService projectService, IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChangeUserRoleToAdminCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeUserRoleProjectAppDto>(request);

        return _projectService.ChangeUserRoleToAdminAsync(dto, ct);
    }
}
