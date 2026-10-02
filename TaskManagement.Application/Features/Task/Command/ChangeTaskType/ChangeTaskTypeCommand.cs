using MediatR;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskType;
public record ChangeTaskTypeCommand(
    long UserId,
    long TaskId
) : IRequest;