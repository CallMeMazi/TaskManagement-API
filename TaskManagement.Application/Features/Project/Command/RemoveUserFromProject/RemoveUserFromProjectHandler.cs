using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.RemoveUserFromProject;
public class RemoveUserFromProjectHandler
    : IRequestHandler<RemoveUserFromProjectCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public RemoveUserFromProjectHandler(IProjectService projectService, IMapper mapper, IUnitOfWork uow)
    {
        _projectService = projectService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(RemoveUserFromProjectCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<AddRemoveUserProjectAppDto>(request);

        var removeUserProjectRes = await _projectService.RemoveUserFromProjectAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return removeUserProjectRes;
    }
}
