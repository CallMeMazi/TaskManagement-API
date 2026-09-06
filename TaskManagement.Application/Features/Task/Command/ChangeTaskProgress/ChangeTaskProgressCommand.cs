using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskProgress;
public record ChangeTaskProgressCommand(
    long UserId,
    long TaskId,
    byte TaskProgress
) : IRequest<GeneralResult>;