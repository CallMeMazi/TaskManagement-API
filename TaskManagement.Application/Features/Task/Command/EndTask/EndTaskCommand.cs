using MediatR;

namespace TaskManagement.Application.Features.Task.Command.EndTask;
public record EndTaskCommand(
    long UserId,
    long TaskId,
    string TaskInfoDescription
) : IRequest;