using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToAdmin;
public class ChangeUserRoleToAdminHandler
    : IRequestHandler<ChangeUserRoleToAdminCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ChangeUserRoleToAdminHandler(IProjectService projectService, IMapper mapper, IUnitOfWork uow)
    {
        _projectService = projectService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeUserRoleToAdminCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeUserRoleProjectAppDto>(request);

        var chagneRoleProjectRes = await _projectService.ChangeUserRoleToAdminAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return chagneRoleProjectRes;
    }
}
