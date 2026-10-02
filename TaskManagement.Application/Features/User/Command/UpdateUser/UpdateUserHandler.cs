using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.User.Command.UpdateUser;

public class UpdateUserHandler
    : IRequestHandler<UpdateUserCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserService _userService;
    private readonly ICommonService _common;

    public UpdateUserHandler(IUserService userService, ICommonService common, IUnitOfWork uow)
    {
        _userService = userService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<UpdateUserAppDto>(request);

        await _userService.UpdateUserAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}