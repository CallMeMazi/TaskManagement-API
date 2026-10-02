using MediatR;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Task.Command.CreateTask;
public record CreateTaskCommand(
    long ProjId,
    long UserId,
    string TaskName,
    string TaskDescription,
    TaskType TaskType,
    DateTime TaskDeadLine,
    List<long> UserIds
) : IRequest;