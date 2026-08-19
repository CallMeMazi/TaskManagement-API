using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.User.Command.CreateUser;
internal class CreateUserValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.MobileNumber).ValidateEmail();

        RuleFor(x => x.Email).ValidateEmail();

        RuleFor(x => x.Password).NotEmpty().WithMessage("رمز عبور نمیتوانید خالی باشد!")
            .Length(8, 256).WithMessage("رمز عبور باید بین 8 و 256 کاراکتر باشد!")
            .NotEqual(x => x.ConfirmPassword).WithMessage("رمز عبور و رمز عبور تایید برابر نیست!");

        RuleFor(x => x.FirstName).NotEmpty().WithMessage("نام نمیتواند خالی باشد!")
            .MaximumLength(50).WithMessage("نام نمیتواند بیشتر از 50 کارارکتر باشد");

        RuleFor(x => x.LastName).NotEmpty().WithMessage("نام خانوادگی نمیتواند خالی باشد!")
            .MaximumLength(50).WithMessage("نام خانوادگی نمیتواند بیشتر از 50 کاراکتر باشد!");

        RuleFor(x => x.DeviceId).ValidateDeviceId();

        RuleFor(x => x.UserIp).ValidateIp();

        RuleFor(x => x.UserAgent).ValidateUserAgent();
    }
}
