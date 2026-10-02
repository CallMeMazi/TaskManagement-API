using MediatR;

namespace TaskManagement.Application.Features.Invitation.Command.RevokeInvitation;
public record RevokeInvitationCommand(
    long OrgOwnerId,
    long InvitationId
) : IRequest;