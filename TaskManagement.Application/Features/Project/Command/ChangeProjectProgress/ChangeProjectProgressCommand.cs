using MediatR;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectProgress;
public record ChangeProjectProgressCommand(
    long OwnerId,
    long ProjId,
    byte ProjectProgress
) : IRequest;