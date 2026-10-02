using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.User.Command.ChangePasswordUser;

public record ChangePasswordUserHandler
    : IRequestHandler<ChangePasswordUserCommand>
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

    public async System.Threading.Tasks.Task Handle(ChangePasswordUserCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangePasswordUserAppDto>(request);

        await _userService.ChangePasswordUserAsync(dto, ct);

        var revokeDto = _common.Mapper.Map<RevokeUserTokenAppDto>(request);

        // revoke all User tokens except current
        await _authService.RevokeAllTokensExceptCurrentByUserIdAsync(revokeDto, ct);

        await _uow.SaveAsync(ct);
    }
}