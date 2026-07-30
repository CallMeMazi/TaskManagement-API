using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskActivity;
public record ChangeTaskActivityCommand(
    int UserId,
    int TaskId,
    bool Activity
) : IRequest<GeneralResult>;