using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.CreateUser;

public class CreateUserHandler
    : IRequestHandler<CreateUserCommand, GeneralResult<UserTokenDto>>
{
    private readonly IUserService _userService;
    private readonly IAuthServiec _authService;
    private readonly IMapper _mapper;

    public CreateUserHandler(IUserService userService, IAuthServiec authServiec, IMapper mapper)
    {
        _userService = userService;
        _authService = authServiec;
        _mapper = mapper;
    }

    public async Task<GeneralResult<UserTokenDto>> Handle(CreateUserCommand request, CancellationToken ct)
    {
        // TransActional

        var dto = _mapper.Map<CreateUserAppDto>(request);

        var createUserRes = await _userService.CreateUserAsync(dto, ct);

        // Generate User tokens(regester) after creation
        return await _authService.RegisterUserAsync
            (new RegisterUserTokenAppDto(createUserRes.Result, request.DeviceId, request.UserIp, request.UserAgent), ct);
    }
}