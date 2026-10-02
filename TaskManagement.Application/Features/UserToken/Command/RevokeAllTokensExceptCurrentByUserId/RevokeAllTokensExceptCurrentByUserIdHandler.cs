using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.UserToken;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensExceptCurrentByUserId;
public class RevokeAllTokensExceptCurrentByUserIdHandler
    : IRequestHandler<RevokeAllTokensExceptCurrentByUserIdCommand>
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

    public async System.Threading.Tasks.Task Handle(RevokeAllTokensExceptCurrentByUserIdCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<RevokeUserTokenAppDto>(request);

        await _authService.RevokeAllTokensExceptCurrentByUserIdAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
