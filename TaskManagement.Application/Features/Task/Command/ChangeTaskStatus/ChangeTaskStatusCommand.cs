using MediatR;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskStatus;
public record ChangeTaskStatusCommand(
    int UserId,
    int TaskId,
    TaskStatusType TaskStatus
) : IRequest<GeneralResult>;