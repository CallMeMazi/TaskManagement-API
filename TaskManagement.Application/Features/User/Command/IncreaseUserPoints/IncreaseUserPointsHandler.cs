using MediatR;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.IncreaseUserPoints;

public class IncreaseUserPointsHandler
    : IRequestHandler<IncreaseUserPointsCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserService _userService;

    public IncreaseUserPointsHandler(IUserService userService, IUnitOfWork uow)
    {
        _userService = userService;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(IncreaseUserPointsCommand request, CancellationToken ct)
    {
        var increaseUserPoint = await _userService.IncreaseUserPointsAsync(request.UserId, ct);

        await _uow.SaveAsync(ct);

        return increaseUserPoint;
    }
}