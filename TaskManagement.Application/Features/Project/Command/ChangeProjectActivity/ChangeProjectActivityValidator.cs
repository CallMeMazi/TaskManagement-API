using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectActivity;
public class ChangeProjectActivityValidator
    : AbstractValidator<ChangeProjectActivityCommand>
{
    public ChangeProjectActivityValidator()
    {
        RuleFor(x => x.OwnerId).ValidateId();

        RuleFor(x => x.ProjId).ValidateId();

        RuleFor(x => x.UserPassword).NotEmpty().WithMessage("رمز عبور نمیتواند خالی باشد!")
            .Length(8, 256).WithMessage("رمز عبور باید بین 8 و 256 کاراکتر باشد!");

        RuleFor(x => x.Activity).NotEmpty().WithMessage("مقدار فعال/غیرفعال نمیتواند خالی باشد!");
    }
}
