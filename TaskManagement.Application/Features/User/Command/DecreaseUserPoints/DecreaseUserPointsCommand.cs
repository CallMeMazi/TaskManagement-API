using MediatR;

namespace TaskManagement.Application.Features.User.Command.DecreaseUserPoints;

public record DecreaseUserPointsCommand(long UserId)
    : IRequest;