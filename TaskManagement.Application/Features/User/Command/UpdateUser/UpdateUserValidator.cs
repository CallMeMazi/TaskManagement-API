using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.User.Command.UpdateUser;
public class UpdateUserValidator
    : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.Email).ValidateEmail();

        RuleFor(x => x.FirstName).NotEmpty().WithMessage("نام نمیتواند خالی باشد!")
             .MaximumLength(50).WithMessage("نام نمیتواند بیشتر از 50 کارارکتر باشد");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("نام خانوادگی نمیتواند خالی باشد!")
            .MaximumLength(50).WithMessage("نام خانوادگی نمیتواند بیشتر از 50 کاراکتر باشد!");
    }
}
