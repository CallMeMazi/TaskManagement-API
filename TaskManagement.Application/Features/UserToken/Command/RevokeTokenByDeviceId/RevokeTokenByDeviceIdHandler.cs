using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeTokenByDeviceId;
public class RevokeTokenByDeviceIdHandler
    : IRequestHandler<RevokeTokenByDeviceIdCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthServiec _authService;
    private readonly ICommonService _common;

    public RevokeTokenByDeviceIdHandler(IAuthServiec authService, ICommonService common, IUnitOfWork uow)
    {
        _authService = authService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(RevokeTokenByDeviceIdCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<RevokeUserTokenAppDto>(request);

        await _authService.RevokeTokenByDeviceIdAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
