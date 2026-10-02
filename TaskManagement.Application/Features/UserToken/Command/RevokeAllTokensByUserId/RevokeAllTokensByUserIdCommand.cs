using MediatR;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensByUserId;
public record RevokeAllTokensByUserIdCommand(long UserId)
    : IRequest;