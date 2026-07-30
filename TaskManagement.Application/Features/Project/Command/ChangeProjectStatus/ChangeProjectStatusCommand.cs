using MediatR;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectStatus;
public record ChangeProjectStatusCommand(
    int OwnerId,
    string UserPassword,
    int ProjId,
    ProjectStatusType ProjectStatus
) : IRequest<GeneralResult>;