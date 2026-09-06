using MediatR;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectStatus;
public record ChangeProjectStatusCommand(
    long OwnerId,
    string UserPassword,
    long ProjId,
    ProjectStatusType ProjectStatus
) : IRequest<GeneralResult>;