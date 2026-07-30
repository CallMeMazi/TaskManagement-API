using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.UpdateTask;
public record UpdateTaskCommand(
    int UserId,
    int TaskId,
    string TaskName,
    string TaskDescription,
    DateTime TaskDeadLine
) : IRequest<GeneralResult>;