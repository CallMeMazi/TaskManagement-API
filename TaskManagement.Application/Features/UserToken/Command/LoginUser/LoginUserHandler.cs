using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.DTOs.ResponseDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.LoginUser;
public class LoginUserHandler
    : IRequestHandler<LoginUserCommand, GeneralResult<UserTokenDto>>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthServiec _authService;
    private readonly IMapper _mapper;

    public LoginUserHandler(IAuthServiec authService, IMapper mapper, IUnitOfWork uow)
    {
        _authService = authService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult<UserTokenDto>> Handle(LoginUserCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<LoginUserAppDto>(request);

        var loginUserRes = await _authService.LoginUserAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return loginUserRes;
    }
}
