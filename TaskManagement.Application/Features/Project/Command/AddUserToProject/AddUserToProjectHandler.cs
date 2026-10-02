using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.AddUserToProject;
public class AddUserToProjectHandler
    : IRequestHandler<AddUserToProjectCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectSerivce;
    private readonly ICommonService _common;

    public AddUserToProjectHandler(IProjectService projectSerivce, ICommonService common, IUnitOfWork uow)
    {
        _projectSerivce = projectSerivce;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(AddUserToProjectCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<AddRemoveUserProjectAppDto>(request);

        var addUserProjectRes = await _projectSerivce.AddUserToProjectAysnc(dto, ct);

        await _uow.SaveAsync(ct);

        return addUserProjectRes;
    }
}
