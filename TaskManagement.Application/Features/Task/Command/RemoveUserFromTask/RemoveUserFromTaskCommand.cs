using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.RemoveUserFromTask;
public record RemoveUserFromTaskCommand(
    int OwnerId,
    int UserId,
    int TaskId,
    int ProjId
) : IRequest<GeneralResult>;