using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.AddUserToProject;
internal record AddUserToProjectCommand(
    int UserId,
    int ProjId,
    int OwnerId
) : IRequest<GeneralResult>;