using AutoMapper;
using TaskManagement.Application.DTOs.ApplicationDTOs.Project;
using TaskManagement.Application.DTOs.SharedDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Main;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Services;
public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<GeneralResult> CancelProjectAsync(UserProjectAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult> ChangeProjectActivityAsync(ChangeProjectActivityAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult> ChangeProjectStatusAsync(ChangeProjectStatusAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult> CreateProjectAsync(CreateProjectAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult> FinishProjectAsync(UserProjectAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult<ProjectDetailsDto>> GetProjectByIdAsync(int projId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult> SoftDeleteProjectAsync(UserProjectAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GeneralResult> UpdateProjectAsync(UpdateProjectAppDto command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
