using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeTokenByDeviceId;
public class RevokeTokenByDeviceIdValidator
    : AbstractValidator<RevokeTokenByDeviceIdCommand>
{
    public RevokeTokenByDeviceIdValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.DeviceId).ValidateDeviceId();
    }
}
