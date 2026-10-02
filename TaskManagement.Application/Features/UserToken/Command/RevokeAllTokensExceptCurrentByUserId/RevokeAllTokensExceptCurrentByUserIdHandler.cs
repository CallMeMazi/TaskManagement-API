using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensExceptCurrentByUserId;
public class RevokeAllTokensExceptCurrentByUserIdHandler
    : IRequestHandler<RevokeAllTokensExceptCurrentByUserIdCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthServiec _authService;
    private readonly ICommonService _common;

    public RevokeAllTokensExceptCurrentByUserIdHandler(IAuthServiec authService, ICommonService common, IUnitOfWork uow)
    {
        _authService = authService;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(RevokeAllTokensExceptCurrentByUserIdCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<RevokeUserTokenAppDto>(request);

        var revokeAllTokens = await _authService.RevokeAllTokensExceptCurrentByUserIdAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return revokeAllTokens;
    }
}
