using MediatR;

namespace TaskManagement.Application.Features.Project.Command.AddUserToProject;
public record AddUserToProjectCommand(
    long UserId,
    long ProjId,
    long OwnerId
) : IRequest;