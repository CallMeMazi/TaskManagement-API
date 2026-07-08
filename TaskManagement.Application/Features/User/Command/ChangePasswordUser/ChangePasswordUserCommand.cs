using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.ChangePasswordUser;

public record ChangePasswordUserCommand(
        int UserId,
        string OldPassword,
        string NewPassword,
        string DeviceId
    ) : IRequest<GeneralResult>;