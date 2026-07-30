using MediatR;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.Features.Project.Command.ChagneProjectStatus;
internal record ChagneProjectStatusCommand(
    int OwnerId,
    string UserPassword,
    int ProjId,
    ProjectStatusType ProjectStatus
) : IRequest<GeneralResult>;