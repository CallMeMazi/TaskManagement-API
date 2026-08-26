using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Invitation.Command.AcceptInvitation;
public class AcceptInvitationValidator
    : AbstractValidator<AcceptInvitationCommand>
{
    public AcceptInvitationValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.Token).NotEmpty().WithMessage("توکن درخواست دعوت نمیتواد خالی باشد!")
            .Length(16, 16).WithMessage("توکن درخواست  دعوت باید 16 کاراکتر باشد!");
    }
}
