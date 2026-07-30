using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.UpdateUser;

public record UpdateUserCommand(
        int UserId,
        string Email,
        string FirstName,
        string LastName
    ) : IRequest<GeneralResult>;