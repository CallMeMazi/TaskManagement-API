using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskType;
public record ChangeTaskTypeCommand(
    long UserId,
    long TaskId
) : IRequest<GeneralResult>;