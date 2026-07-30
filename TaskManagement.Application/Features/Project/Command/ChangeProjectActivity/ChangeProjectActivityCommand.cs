using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectActivity;
internal record ChangeProjectActivityCommand(
    int OwnerId,
    int ProjId,
    string UserPassword,
    bool Activity
) : IRequest<GeneralResult>;