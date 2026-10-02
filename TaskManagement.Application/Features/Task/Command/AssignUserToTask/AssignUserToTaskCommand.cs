using MediatR;

namespace TaskManagement.Application.Features.Task.Command.AssignUserToTask;
public record AssignUserToTaskCommand(
    long OwnerId,
    long UserId,
    long TaskId,
    long ProjId
) : IRequest;