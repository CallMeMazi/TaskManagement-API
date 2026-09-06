using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.User.Command.DeleteUser;

public record DeleteUserCommand(
    long UserId,
    string password
) : IRequest<GeneralResult>;