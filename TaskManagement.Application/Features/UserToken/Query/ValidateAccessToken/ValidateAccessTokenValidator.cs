using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.UserToken.Query.ValidateAccessToken;
public class ValidateAccessTokenValidator
    : AbstractValidator<ValidateAcceessTokenQuery>
{
    public ValidateAccessTokenValidator()
    {
        RuleFor(x => x.AccessToken).NotEmpty().WithMessage("The access token can not be empty!");

        RuleFor(x => x.DeviceId).ValidateDeviceId();
    }
}
