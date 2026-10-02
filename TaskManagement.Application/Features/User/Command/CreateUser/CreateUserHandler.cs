using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.CreateUser;

public class CreateUserHandler
    : IRequestHandler<CreateUserCommand, GeneralResult<UserTokenDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserService _userService;
    private readonly IAuthServiec _authService;
    private readonly ICommonService _common;

    public CreateUserHandler(IUnitOfWork uow, IUserService userService, IAuthServiec authServiec
        , ICommonService common)
    {
        _uow = uow;
        _userService = userService;
        _authService = authServiec;
        _common = common;
    }

    public async Task<GeneralResult<UserTokenDto>> Handle(CreateUserCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<CreateUserAppDto>(request);

        // Create User And Return UserID
        var createUserRes = await _userService.CreateUserAsync(dto, ct);

        // Generate User tokens(regester) after creation
        var generateUserTokenRes = await _authService.RegisterUserAsync
            (new RegisterUserTokenAppDto(createUserRes.Result, request.DeviceId, request.UserIp, request.UserAgent), ct);

        await _uow.SaveAsync(ct);

        return generateUserTokenRes;
    }
}