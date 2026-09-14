using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.AddUserToProject;
public class AddUserToProjectHandler
    : IRequestHandler<AddUserToProjectCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectSerivce;
    private readonly IMapper _mapper;

    public AddUserToProjectHandler(IProjectService projectSerivce, IMapper mapper, IUnitOfWork uow)
    {
        _projectSerivce = projectSerivce;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(AddUserToProjectCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<AddRemoveUserProjectAppDto>(request);

        var addUserProjectRes = await _projectSerivce.AddUserToProjectAysnc(dto, ct);

        await _uow.SaveAsync(ct);

        return addUserProjectRes;
    }
}
