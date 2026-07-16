using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensExceptCurrentByUserId;
public record RevokeAllTokensExceptCurrentByUserIdCommand(int UserId, string DeviceId)
    : IRequest<GeneralResult>;