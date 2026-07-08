using MediatR;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.DecreaseUserPoints;

public class DecreaseUserPointsHandler
    : IRequestHandler<DecreaseUserPointsCommand, GeneralResult>
{
    private readonly IUserService _userService;

    public DecreaseUserPointsHandler(IUserService userService) => _userService = userService;

    public Task<GeneralResult> Handle(DecreaseUserPointsCommand request, CancellationToken ct)
    {
        return _userService.DecreaseUserPointsAsync(request.UserId, ct);
    }
}