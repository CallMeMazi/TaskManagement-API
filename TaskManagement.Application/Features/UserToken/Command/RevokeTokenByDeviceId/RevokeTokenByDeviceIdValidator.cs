using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeTokenByDeviceId;
internal class RevokeTokenByDeviceIdValidator
    : AbstractValidator<RevokeTokenByDeviceIdCommand>
{
    public RevokeTokenByDeviceIdValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.DeviceId).ValidateDeviceId();
    }
}
