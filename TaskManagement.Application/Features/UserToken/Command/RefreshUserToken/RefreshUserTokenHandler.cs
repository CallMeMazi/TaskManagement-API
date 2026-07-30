using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Application.Features.UserToken.Command.RefreshToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RefreshUserToken;
public class RefreshUserTokenHandler
    : IRequestHandler<RefreshUserTokenCommand, GeneralResult<UserTokenDto>>
{
    private readonly IAuthServiec _authService;
    private readonly IMapper _mapper;

    public RefreshUserTokenHandler(IAuthServiec authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public Task<GeneralResult<UserTokenDto>> Handle(RefreshUserTokenCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<RefreshUserTokenAppDto>(request);

        return _authService.RefreshTokenAsync(dto, ct);
    }
}
