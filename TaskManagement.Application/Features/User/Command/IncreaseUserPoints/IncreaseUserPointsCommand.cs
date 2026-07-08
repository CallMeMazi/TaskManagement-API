using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.IncreaseUserPoints;

public record IncreaseUserPointsCommand(int UserId)
    : IRequest<GeneralResult>;