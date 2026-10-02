using MediatR;

namespace TaskManagement.Application.Features.Invitation.Command.AcceptInvitation;
public record AcceptInvitationCommand(
    long UserId,
    string Token
) : IRequest;