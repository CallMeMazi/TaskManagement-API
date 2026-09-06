using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.AddUserToProject;
public record AddUserToProjectCommand(
    long UserId,
    long ProjId,
    long OwnerId
) : IRequest<GeneralResult>;