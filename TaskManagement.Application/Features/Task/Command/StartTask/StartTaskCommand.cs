using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.StartTask;
public record StartTaskCommand(
    long UserId,
    long TaskId
) : IRequest<GeneralResult>;