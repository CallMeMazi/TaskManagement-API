using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.ChangePasswordUser;

public record ChangePasswordUserHandler
    : IRequestHandler<ChangePasswordUserCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserService _userService;
    private readonly IAuthServiec _authService;
    private readonly ICommonService _common;

    public ChangePasswordUserHandler(IUserService userService, IAuthServiec authServiec, ICommonService common
        , IUnitOfWork uow)
    {
        _userService = userService;
        _authService = authServiec;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangePasswordUserCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangePasswordUserAppDto>(request);

        await _userService.ChangePasswordUserAsync(dto, ct);

        // revoke all User tokens except current
        var changePassUserRes = await _authService.RevokeAllTokensExceptCurrentByUserIdAsync
            (new RevokeUserTokenAppDto(request.UserId, request.DeviceId), ct);

        await _uow.SaveAsync(ct);

        return changePassUserRes;
    }
}