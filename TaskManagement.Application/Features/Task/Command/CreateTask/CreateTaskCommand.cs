using MediatR;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Features.Task.Command.CreateTask;
public record CreateTaskCommand(
    long ProjId,
    long UserId,
    string TaskName,
    string TaskDescription,
    TaskType TaskType,
    DateTime TaskDeadLine,
    List<long> UserIds
) : IRequest<GeneralResult>;