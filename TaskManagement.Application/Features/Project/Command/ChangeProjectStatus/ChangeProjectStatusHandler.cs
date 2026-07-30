using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectStatus;
public class ChangeProjectStatusHandler
    : IRequestHandler<ChangeProjectStatusCommand, GeneralResult>
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ChangeProjectStatusHandler(IProjectService projectService, IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChangeProjectStatusCommand request, CancellationToken ct)
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
                throw new ArgumentException($"Error in {nameof(ChangeProjectStatusHandler)} Handler!");
        }
    }
}
