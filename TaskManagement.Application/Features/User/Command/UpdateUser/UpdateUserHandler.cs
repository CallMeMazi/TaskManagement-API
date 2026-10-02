using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.UpdateUser;

public class UpdateUserHandler
    : IRequestHandler<UpdateUserCommand, GeneralResult>
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

    public async Task<GeneralResult> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<UpdateUserAppDto>(request);

        var updateUserRes = await _userService.UpdateUserAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return updateUserRes;
    }
}