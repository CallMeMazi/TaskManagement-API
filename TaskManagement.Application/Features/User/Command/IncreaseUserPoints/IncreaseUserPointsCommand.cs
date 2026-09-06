using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.IncreaseUserPoints;

public record IncreaseUserPointsCommand(long UserId)
    : IRequest<GeneralResult>;