using MediatR;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskProgress;
public record ChangeTaskProgressCommand(
    long UserId,
    long TaskId,
    byte TaskProgress
) : IRequest;