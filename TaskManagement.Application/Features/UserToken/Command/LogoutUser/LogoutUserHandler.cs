using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.LogoutUser;
public class LogoutUserHandler
    : IRequestHandler<LogoutUserCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthServiec _authService;
    private readonly IMapper _mapper;

    public LogoutUserHandler(IAuthServiec authService, IMapper mapper, IUnitOfWork uow)
    {
        _authService = authService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(LogoutUserCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<LogoutUserAppDto>(request);

        var loguotUserRes = await _authService.LogoutUserAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return loguotUserRes;
    }
}
