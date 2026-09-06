using MediatR;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskStatus;
public record ChangeTaskStatusCommand(
    long UserId,
    long TaskId,
    TaskStatusType TaskStatus
) : IRequest<GeneralResult>;