using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Command.RevokeInvitation;
public record RevokeInvitationCommand(
    long OrgOwnerId,
    long InvitationId
) : IRequest<GeneralResult>;