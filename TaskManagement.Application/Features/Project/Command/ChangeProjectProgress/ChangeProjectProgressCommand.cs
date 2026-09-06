using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectProgress;
public record ChangeProjectProgressCommand(
    long OwnerId,
    long ProjId,
    byte ProjectProgress
) : IRequest<GeneralResult>;