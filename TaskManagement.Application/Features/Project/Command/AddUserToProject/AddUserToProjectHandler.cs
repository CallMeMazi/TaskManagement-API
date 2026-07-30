using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.AddUserToProject;
internal class AddUserToProjectHandler
    : IRequestHandler<AddUserToProjectCommand, GeneralResult>
{
    private readonly IProjectService _projectSerivce;
    private readonly IMapper _mapper;

    public AddUserToProjectHandler(IProjectService projectSerivce, IMapper mapper)
    {
        _projectSerivce = projectSerivce;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(AddUserToProjectCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<AddRemoveUserProjectAppDto>(request);

        return _projectSerivce.AddUserToProjectAysnc(dto, ct);
    }
}
