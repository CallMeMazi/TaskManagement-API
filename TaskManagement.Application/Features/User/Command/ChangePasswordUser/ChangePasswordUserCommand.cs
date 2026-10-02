using MediatR;

namespace TaskManagement.Application.Features.User.Command.ChangePasswordUser;

public record ChangePasswordUserCommand(
    long UserId,
    string OldPassword,
    string NewPassword,
    string ConfirmPassword,
    string DeviceId
) : IRequest;