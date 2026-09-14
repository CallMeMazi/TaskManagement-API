using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectStatus;
public class ChangeProjectStatusHandler
    : IRequestHandler<ChangeProjectStatusCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ChangeProjectStatusHandler(IProjectService projectService, IMapper mapper, IUnitOfWork uow)
    {
        _projectService = projectService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeProjectStatusCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UserProjectAppDto>(request);

        GeneralResult changeProjectStatus;
        switch (request.ProjectStatus)
        {
            case ProjectStatusType.InProgress:
                changeProjectStatus = await _projectService.ChangeProjectStatusToInProgressAsync(dto, ct);
                break;
            case ProjectStatusType.Adjournment:
                changeProjectStatus = await _projectService.ChangeProjectStatusToAdjournmentAsync(dto, ct);
                break;
            case ProjectStatusType.Cancel:
                changeProjectStatus = await _projectService.CancelProjectAsync(dto, ct);
                break;
            case ProjectStatusType.Finished:
                changeProjectStatus = await _projectService.FinishProjectAsync(dto, ct);
                break;
            default:
                throw new ArgumentException($"Error in {nameof(ChangeProjectStatusHandler)} Handler!");
        }

        await _uow.SaveAsync(ct);

        return changeProjectStatus;
    }
}
