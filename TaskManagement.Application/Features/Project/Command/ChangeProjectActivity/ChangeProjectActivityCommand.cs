using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectActivity;
public record ChangeProjectActivityCommand(
    int OwnerId,
    int ProjId,
    string UserPassword,
    bool Activity
) : IRequest<GeneralResult>;