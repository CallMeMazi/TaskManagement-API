using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskType;
public record ChangeTaskTypeCommand(
    int UserId,
    int TaskId
) : IRequest<GeneralResult>;