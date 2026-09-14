using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.CreateProject;
public class CreateProjectHandler
    : IRequestHandler<CreateProjectCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public CreateProjectHandler(IProjectService projectService, IMapper mapper, IUnitOfWork uow)
    {
        _projectService = projectService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(CreateProjectCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<CreateProjectAppDto>(request);

        var createProjectRes = await _projectService.CreateProjectAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return createProjectRes;
    }
}
