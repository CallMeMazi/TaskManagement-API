using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Project.Command.AddUserToProject;
public class AddUserToProjectHandler
    : IRequestHandler<AddUserToProjectCommand>
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

    public async System.Threading.Tasks.Task Handle(AddUserToProjectCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<AddRemoveUserProjectAppDto>(request);

        await _projectSerivce.AddUserToProjectAysnc(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
