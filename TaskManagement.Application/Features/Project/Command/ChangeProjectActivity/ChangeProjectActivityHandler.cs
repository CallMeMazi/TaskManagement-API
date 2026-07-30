using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectActivity;
internal class ChangeProjectActivityHandler
    : IRequestHandler<ChangeProjectActivityCommand, GeneralResult>
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ChangeProjectActivityHandler(IProjectService projectService, IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChangeProjectActivityCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeProjectActivityAppDto>(request);

        return _projectService.ChangeProjectActivityAsync(dto, ct);
    }
}
