using MediatR;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectActivity;
public record ChangeProjectActivityCommand(
    long OwnerId,
    long ProjId,
    string UserPassword,
    bool Activity
) : IRequest;