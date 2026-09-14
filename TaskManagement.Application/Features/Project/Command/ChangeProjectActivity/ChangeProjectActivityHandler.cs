using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectActivity;
public class ChangeProjectActivityHandler
    : IRequestHandler<ChangeProjectActivityCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ChangeProjectActivityHandler(IProjectService projectService, IMapper mapper, IUnitOfWork uow)
    {
        _projectService = projectService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeProjectActivityCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeProjectActivityAppDto>(request);

        var chagneActivityProjectRes = await _projectService.ChangeProjectActivityAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return chagneActivityProjectRes;
    }
}
