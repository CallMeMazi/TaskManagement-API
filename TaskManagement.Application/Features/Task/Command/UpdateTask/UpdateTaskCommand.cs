using MediatR;

namespace TaskManagement.Application.Features.Task.Command.UpdateTask;
public record UpdateTaskCommand(
    long UserId,
    long TaskId,
    string TaskName,
    string TaskDescription,
    DateTime TaskDeadLine
) : IRequest;