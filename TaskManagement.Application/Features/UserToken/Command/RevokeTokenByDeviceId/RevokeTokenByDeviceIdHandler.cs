using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeTokenByDeviceId;
public class RevokeTokenByDeviceIdHandler
    : IRequestHandler<RevokeTokenByDeviceIdCommand, GeneralResult>
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

    public async Task<GeneralResult> Handle(RevokeTokenByDeviceIdCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<RevokeUserTokenAppDto>(request);

        var revokeTokenRes = await _authService.RevokeTokenByDeviceIdAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return revokeTokenRes;
    }
}
