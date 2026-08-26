using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectStatus;
public class ChangeProjectStatusValidator
    : AbstractValidator<ChangeProjectStatusCommand>
{
    public ChangeProjectStatusValidator()
    {
        RuleFor(x => x.OwnerId).ValidateId();

        RuleFor(x => x.ProjId).ValidateId();

        RuleFor(x => x.UserPassword).NotEmpty().WithMessage("رمز عبور نمیتواند خالی باشد!")
            .Length(8, 256).WithMessage("رمز عبور باید بین 8 و 256 کاراکتر باشد!");

        RuleFor(x => x.ProjectStatus).NotEmpty().WithMessage("")
            .IsInEnum().WithMessage("مقدار وضعیت پروژه نامعتبر است!");
    }
}
