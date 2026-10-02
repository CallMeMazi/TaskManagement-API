using MediatR;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeTokenByDeviceId;
public record RevokeTokenByDeviceIdCommand(long UserId, string DeviceId)
    : IRequest;