using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Command.GenerateInviteLinkByUserId;
public record GenerateInviteLinkByUserIdCommand(
    int OrgId,
    int OrgOwnerId,
    string UserMobileNumber
) : IRequest<GeneralResult<string>>;
