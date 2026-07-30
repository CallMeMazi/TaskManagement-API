using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.DeleteTask;
public record DeleteTaskCommand(
    int UserId,
    int TaskId
) : IRequest<GeneralResult>;