using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.DeleteTask;
public record DeleteTaskCommand(
    long UserId,
    long TaskId
) : IRequest<GeneralResult>;