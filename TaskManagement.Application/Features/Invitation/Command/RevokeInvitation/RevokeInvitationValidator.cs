using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Invitation.Command.RevokeInvitation;
public class RevokeInvitationValidator
    : AbstractValidator<RevokeInvitationCommand>
{
    public RevokeInvitationValidator()
    {
        RuleFor(x => x.OrgOwnerId).ValidateId();

        RuleFor(x => x.InvitationId).ValidateId();
    }
}
