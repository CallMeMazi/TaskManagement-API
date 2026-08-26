using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Organization.Command.ChangeOrgActivity;
public class ChangeOrgActivityValidator
    : AbstractValidator<ChangeOrgActivityCommand>
{
    public ChangeOrgActivityValidator()
    {
        RuleFor(x => x.OwnerId).ValidateId();

        RuleFor(x => x.OrgId).ValidateId();

        RuleFor(x => x.OwnerPassword).NotEmpty().WithMessage("رمز عبور نمیتواند خالی باشد!")
            .Length(8, 256).WithMessage("رمز عبور باید بین 8 و 256 کاراکتر باشد!");
    }
}
