using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensByUserId;
public record RevokeAllTokensByUserIdCommand(int UserId)
    : IRequest<GeneralResult>;