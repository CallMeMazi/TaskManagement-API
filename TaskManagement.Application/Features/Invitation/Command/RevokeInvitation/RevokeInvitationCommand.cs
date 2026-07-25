using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Command.RevokeInvitation;
public record RevokeInvitationCommand(
    int OrgOwnerId,
    int InvitationId
) : IRequest<GeneralResult>;