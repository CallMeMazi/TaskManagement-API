using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Invitation.Command.GenerateInviteLinkByUserId;
public class GenerateInviteLinkByUserIdValidator
    : AbstractValidator<GenerateInviteLinkByUserIdCommand>
{
    public GenerateInviteLinkByUserIdValidator()
    {
        RuleFor(x => x.OrgId).ValidateId();

        RuleFor(x => x.OrgOwnerId).ValidateId();

        RuleFor(x => x.UserMobileNumber).ValidateMobileNumber();
    }
}
