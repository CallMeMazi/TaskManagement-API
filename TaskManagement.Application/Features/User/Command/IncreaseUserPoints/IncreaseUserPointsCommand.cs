using MediatR;

namespace TaskManagement.Application.Features.User.Command.IncreaseUserPoints;

public record IncreaseUserPointsCommand(long UserId)
    : IRequest;