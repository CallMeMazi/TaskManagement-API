using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskProgress;
public record ChangeTaskProgressCommand(
    int UserId,
    int TaskId,
    byte TaskProgress
) : IRequest<GeneralResult>;