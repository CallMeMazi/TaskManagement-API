using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.StartTask;
public record StartTaskCommand(
    int UserId,
    int TaskId
) : IRequest<GeneralResult>;