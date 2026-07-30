using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectProgress;
internal class ChangeProjectProgressHandler
    : IRequestHandler<ChangeProjectProgressCommand, GeneralResult>
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ChangeProjectProgressHandler(IProjectService projectService, IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChangeProjectProgressCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeProjectProgressAppDto>(request);

        return _projectService.ChangeProjectProgressAsync(dto, ct);
    }
}
