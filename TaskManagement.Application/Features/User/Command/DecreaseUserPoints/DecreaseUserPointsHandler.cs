using MediatR;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.DecreaseUserPoints;

public class DecreaseUserPointsHandler
    : IRequestHandler<DecreaseUserPointsCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserService _userService;

    public DecreaseUserPointsHandler(IUserService userService, IUnitOfWork uow)
    {
        _userService = userService;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(DecreaseUserPointsCommand request, CancellationToken ct)
    {
        var decreaseUserPointRes = await _userService.DecreaseUserPointsAsync(request.UserId, ct);

        await _uow.SaveAsync(ct);

        return decreaseUserPointRes;
    }
}