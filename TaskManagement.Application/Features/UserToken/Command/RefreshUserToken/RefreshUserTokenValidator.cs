using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.UserToken.Command.RefreshUserToken;
public class RefreshUserTokenValidator
    : AbstractValidator<RefreshUserTokenCommand>
{
    public RefreshUserTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("The refresh token can not be empty!")
            .MaximumLength(100).WithMessage("The length of the refresh token must be less then 100!");

        RuleFor(x => x.DeviceId).ValidateDeviceId();
    }
}
