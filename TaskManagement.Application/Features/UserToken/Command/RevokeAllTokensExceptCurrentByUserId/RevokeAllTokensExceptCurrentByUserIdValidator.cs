using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.UserToken.Command.RevokeAllTokensExceptCurrentByUserId;
public class RevokeAllTokensExceptCurrentByUserIdValidator
    : AbstractValidator<RevokeAllTokensExceptCurrentByUserIdCommand>
{
    public RevokeAllTokensExceptCurrentByUserIdValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.DeviceId).ValidateDeviceId();
    }
}
