using MediatR;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.IncreaseUserPoints;

public class IncreaseUserPointsHandler
    : IRequestHandler<IncreaseUserPointsCommand, GeneralResult>
{
    private readonly IUserService _userService;

    public IncreaseUserPointsHandler(IUserService userService) => _userService = userService;

    public Task<GeneralResult> Handle(IncreaseUserPointsCommand request, CancellationToken ct)
    {
        return _userService.IncreaseUserPointsAsync(request.UserId, ct);
    }
}