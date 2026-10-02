using MediatR;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.User.Command.DecreaseUserPoints;

public class DecreaseUserPointsHandler
    : IRequestHandler<DecreaseUserPointsCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserService _userService;

    public DecreaseUserPointsHandler(IUserService userService, IUnitOfWork uow)
    {
        _userService = userService;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(DecreaseUserPointsCommand request, CancellationToken ct)
    {
        await _userService.DecreaseUserPointsAsync(request.UserId, ct);

        await _uow.SaveAsync(ct);
    }
}