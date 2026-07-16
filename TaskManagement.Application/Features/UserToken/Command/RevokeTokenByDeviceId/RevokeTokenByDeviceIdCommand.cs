using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeTokenByDeviceId;
public record RevokeTokenByDeviceIdCommand(int UserId, int DeviceId)
    : IRequest<GeneralResult>;