using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.LoginUser;
public class LoginUserHandler
    : IRequestHandler<LoginUserCommand, GeneralResult<UserTokenDto>>
{
    private readonly IAuthServiec _authService;
    private readonly IMapper _mapper;

    public LoginUserHandler(IAuthServiec authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public Task<GeneralResult<UserTokenDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var dto = _mapper.Map<LoginUserAppDto>(request);

        return _authService.LoginUserAsync(dto, cancellationToken);
    }
}
