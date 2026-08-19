using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.UserToken.Command.LoginUser;
internal class LoginUserValidator
    : AbstractValidator<LoginUserCommand>
{
    public LoginUserValidator()
    {
        RuleFor(x => x.MobileNumber).ValidateMobileNumber();

        RuleFor(x => x.Password).NotEmpty().WithMessage("رمز عبور نمیتوانید خالی باشد!")
            .Length(8, 256).WithMessage("رمز عبور باید بین 8 و 256 کاراکتر باشد!");

        RuleFor(x => x.DeviceId).ValidateDeviceId();

        RuleFor(x => x.UserIp).ValidateIp();

        RuleFor(x => x.UserAgent).ValidateUserAgent();
    }
}
