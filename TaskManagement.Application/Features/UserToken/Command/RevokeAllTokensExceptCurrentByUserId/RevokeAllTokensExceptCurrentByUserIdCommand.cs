using MediatR;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensExceptCurrentByUserId;
public record RevokeAllTokensExceptCurrentByUserIdCommand(long UserId, string DeviceId)
    : IRequest;