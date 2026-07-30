using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.Features.Project.Command.ChagneProjectStatus;
internal class ChagneProjectStatusHandler
    : IRequestHandler<ChagneProjectStatusCommand, GeneralResult>
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ChagneProjectStatusHandler(IProjectService projectService, IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChagneProjectStatusCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UserProjectAppDto>(request);

        switch (request.ProjectStatus)
        {
            case ProjectStatusType.InProgress:
                return _projectService.ChangeProjectStatusToInProgressAsync(dto, ct);
            case ProjectStatusType.Adjournment:
                return _projectService.ChangeProjectStatusToAdjournmentAsync(dto, ct);
            case ProjectStatusType.Cancel:
                return _projectService.CancelProjectAsync(dto, ct);
            case ProjectStatusType.Finished:
                return _projectService.FinishProjectAsync(dto, ct);
            default:
                throw new ArgumentException($"Error in {nameof(ChagneProjectStatusHandler)} Handler!");
        }
    }
}
