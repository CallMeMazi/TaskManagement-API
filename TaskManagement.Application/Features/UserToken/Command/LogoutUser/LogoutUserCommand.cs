using MediatR;

namespace TaskManagement.Application.Features.UserToken.Command.LogoutUser;
public record LogoutUserCommand(
    long UserId,
    string AccessToken,
    string DeviceId
) : IRequest;