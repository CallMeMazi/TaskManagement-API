using MediatR;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Features.Task.Command.CreateTask;
public record CreateTaskCommand(
    int ProjId,
    int UserId,
    string TaskName,
    string TaskDescription,
    TaskType TaskType,
    DateTime TaskDeadLine,
    List<int> UserIds
) : IRequest<GeneralResult>;