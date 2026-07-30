using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.AssignUserToTask;
public record AssignUserToTaskCommand(
    int OwnerId,
    int UserId,
    int TaskId,
    int ProjId
) : IRequest<GeneralResult>;