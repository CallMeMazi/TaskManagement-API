using MediatR;

namespace TaskManagement.Application.Features.User.Command.UpdateUser;

public record UpdateUserCommand(
    long UserId,
    string Email,
    string FirstName,
    string LastName
) : IRequest;