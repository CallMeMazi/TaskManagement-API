using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.UserToken.Command.LogoutUser;
internal class LogoutUserValidator
    : AbstractValidator<LogoutUserCommand>
{
    public LogoutUserValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.AccessToken).NotEmpty().WithMessage("The access token can not be empty!");

        RuleFor(x => x.DeviceId).ValidateDeviceId();
    }
}
