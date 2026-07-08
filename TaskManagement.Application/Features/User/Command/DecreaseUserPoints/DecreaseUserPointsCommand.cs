using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.DecreaseUserPoints;

public record DecreaseUserPointsCommand(int UserId)
    : IRequest<GeneralResult>;