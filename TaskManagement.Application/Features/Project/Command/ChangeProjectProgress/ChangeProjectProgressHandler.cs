using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectProgress;
public class ChangeProjectProgressHandler
    : IRequestHandler<ChangeProjectProgressCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ChangeProjectProgressHandler(IProjectService projectService, IMapper mapper, IUnitOfWork uow)
    {
        _projectService = projectService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeProjectProgressCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeProjectProgressAppDto>(request);

        var changeProjectProgressRes = await _projectService.ChangeProjectProgressAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return changeProjectProgressRes;
    }
}
