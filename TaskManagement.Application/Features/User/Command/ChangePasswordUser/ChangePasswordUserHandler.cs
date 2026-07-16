using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.User;
using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.ChangePasswordUser;

public record ChangePasswordUserHandler
    : IRequestHandler<ChangePasswordUserCommand, GeneralResult>
{
    private readonly IUserService _userService;
    private readonly IAuthServiec _authService;
    private readonly IMapper _mapper;

    public ChangePasswordUserHandler(IUserService userService, IAuthServiec authServiec, IMapper mapper)
    {
        _userService = userService;
        _authService = authServiec;
        _mapper = mapper;
    }

    public async Task<GeneralResult> Handle(ChangePasswordUserCommand request, CancellationToken ct)
    {
        // TransActional

        var dto = _mapper.Map<ChangePasswordUserAppDto>(request);

        await _userService.ChangePasswordUserAsync(dto, ct);

        // revoke all User tokens except current
        return await _authService.RevokeAllTokensExceptCurrentByUserIdAsync
            (new RevokeUserTokenAppDto(request.UserId, request.DeviceId), false, ct);
    }
}