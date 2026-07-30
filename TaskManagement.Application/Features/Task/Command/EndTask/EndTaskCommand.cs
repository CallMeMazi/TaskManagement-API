using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.EndTask;
public record EndTaskCommand(
    int UserId,
    int TaskId
) : IRequest<GeneralResult>;