using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.UserToken.Command.LogoutUser;
public class LogoutUserHandler
    : IRequestHandler<LogoutUserCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthServiec _authService;
    private readonly ICommonService _common;

    public LogoutUserHandler(IAuthServiec authService, ICommonService common, IUnitOfWork uow)
    {
        _authService = authService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(LogoutUserCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<LogoutUserAppDto>(request);

        await _authService.LogoutUserAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
