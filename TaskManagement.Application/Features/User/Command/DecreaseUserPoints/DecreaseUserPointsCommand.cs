using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.DecreaseUserPoints;

public record DecreaseUserPointsCommand(long UserId)
    : IRequest<GeneralResult>;