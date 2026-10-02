using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.LoginUser;
public class LoginUserHandler
    : IRequestHandler<LoginUserCommand, GeneralResult<UserTokenDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthServiec _authService;
    private readonly ICommonService _common;

    public LoginUserHandler(IAuthServiec authService, ICommonService common, IUnitOfWork uow)
    {
        _authService = authService;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult<UserTokenDto>> Handle(LoginUserCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<LoginUserAppDto>(request);

        var loginUserRes = await _authService.LoginUserAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return loginUserRes;
    }
}
