using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.User;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.ChangePasswordUser;

public record ChangePasswordUserHandler
    : IRequestHandler<ChangePasswordUserCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserService _userService;
    private readonly IAuthServiec _authService;
    private readonly IMapper _mapper;

    public ChangePasswordUserHandler(IUserService userService, IAuthServiec authServiec, IMapper mapper, IUnitOfWork uow)
    {
        _userService = userService;
        _authService = authServiec;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangePasswordUserCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangePasswordUserAppDto>(request);

        await _userService.ChangePasswordUserAsync(dto, ct);

        // revoke all User tokens except current
        var changePassUserRes = await _authService.RevokeAllTokensExceptCurrentByUserIdAsync
            (new RevokeUserTokenAppDto(request.UserId, request.DeviceId), ct);

        await _uow.SaveAsync(ct);

        return changePassUserRes;
    }
}