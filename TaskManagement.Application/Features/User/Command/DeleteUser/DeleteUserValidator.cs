using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.User.Command.DeleteUser;
public class DeleteUserValidator
    : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.password).NotEmpty().WithMessage("رمز عبور نمیتوانید خالی باشد!")
            .Length(8, 256).WithMessage("رمز عبور باید بین 8 و 256 کاراکتر باشد!");
    }
}
