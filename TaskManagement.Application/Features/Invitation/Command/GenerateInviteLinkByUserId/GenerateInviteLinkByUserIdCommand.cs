using MediatR;

namespace TaskManagement.Application.Features.Invitation.Command.GenerateInviteLinkByUserId;
public record GenerateInviteLinkByUserIdCommand(
    long OrgId,
    long OrgOwnerId,
    string UserMobileNumber
) : IRequest<string>;
