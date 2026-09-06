using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskActivity;
public record ChangeTaskActivityCommand(
    long UserId,
    long TaskId,
    bool Activity
) : IRequest<GeneralResult>;