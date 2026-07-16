using MediatR;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensByUserId;
public class RevokeAllTokensByUserIdHandler
    : IRequestHandler<RevokeAllTokensByUserIdCommand, GeneralResult>
{
    private readonly IAuthServiec _authService;

    public RevokeAllTokensByUserIdHandler(IAuthServiec authService)
    {
        _authService = authService;
    }

    public Task<GeneralResult> Handle(RevokeAllTokensByUserIdCommand request, CancellationToken ct)
        => _authService.RevokeAllTokensByUserIdAsync(request.UserId, true, ct);
}
