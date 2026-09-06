using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensExceptCurrentByUserId;
public record RevokeAllTokensExceptCurrentByUserIdCommand(long UserId, string DeviceId)
    : IRequest<GeneralResult>;