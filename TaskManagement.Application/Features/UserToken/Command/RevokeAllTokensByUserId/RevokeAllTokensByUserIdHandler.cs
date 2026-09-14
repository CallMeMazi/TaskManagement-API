using MediatR;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensByUserId;
public class RevokeAllTokensByUserIdHandler
    : IRequestHandler<RevokeAllTokensByUserIdCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthServiec _authService;

    public RevokeAllTokensByUserIdHandler(IAuthServiec authService, IUnitOfWork uow)
    {
        _authService = authService;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(RevokeAllTokensByUserIdCommand request, CancellationToken ct)
    {
        var revokeAllTokens = await _authService.RevokeAllTokensByUserIdAsync(request.UserId, ct);

        await _uow.SaveAsync(ct);

        return revokeAllTokens;
    }
}
