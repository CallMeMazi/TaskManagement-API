using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.RemoveUserFromTask;
public record RemoveUserFromTaskCommand(
    long OwnerId,
    long UserId,
    long TaskId,
    long ProjId
) : IRequest<GeneralResult>;