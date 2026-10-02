using MediatR;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensByUserId;
public class RevokeAllTokensByUserIdHandler
    : IRequestHandler<RevokeAllTokensByUserIdCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthServiec _authService;

    public RevokeAllTokensByUserIdHandler(IAuthServiec authService, IUnitOfWork uow)
    {
        _authService = authService;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(RevokeAllTokensByUserIdCommand request, CancellationToken ct)
    {
        await _authService.RevokeAllTokensByUserIdAsync(request.UserId, ct);

        await _uow.SaveAsync(ct);
    }
}
