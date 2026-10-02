using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.DeleteUser;

public class DeleteUserHandler
    : IRequestHandler<DeleteUserCommand, GeneralResult>
{
    private readonly IUserService _userService;
    private readonly ICommonService _common;

    public DeleteUserHandler(IUserService userService, ICommonService common)
    {
        _userService = userService;
        _common = common;
    }

    public Task<GeneralResult> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<DeleteUserAppDto>(request);

        return _userService.SoftDeleteUserAsync(dto, ct);
    }
}