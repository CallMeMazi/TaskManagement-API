using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.UpdateProject;
public class UpdateProjectHandler
    : IRequestHandler<UpdateProjectCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public UpdateProjectHandler(IProjectService projectService, IMapper mapper, IUnitOfWork uow)
    {
        _projectService = projectService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(UpdateProjectCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UpdateProjectAppDto>(request);

        var updateProjectRes = await _projectService.UpdateProjectAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return updateProjectRes;
    }
}
