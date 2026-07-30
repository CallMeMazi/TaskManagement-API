using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectProgress;
internal record ChangeProjectProgressCommand(
    int OwnerId,
    int ProjId,
    byte ProjectProgress
) : IRequest<GeneralResult>;