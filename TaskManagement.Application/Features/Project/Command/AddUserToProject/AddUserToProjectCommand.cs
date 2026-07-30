using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.AddUserToProject;
public record AddUserToProjectCommand(
    int UserId,
    int ProjId,
    int OwnerId
) : IRequest<GeneralResult>;