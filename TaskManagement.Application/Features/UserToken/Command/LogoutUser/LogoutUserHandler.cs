using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.LogoutUser;
public class LogoutUserHandler
    : IRequestHandler<LogoutUserCommand, GeneralResult>
{
    private readonly IAuthServiec _authService;
    private readonly IMapper _mapper;

    public LogoutUserHandler(IAuthServiec authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(LogoutUserCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<LogoutUserAppDto>(request);

        return _authService.LogoutUserAsync(dto, ct);
    }
}
