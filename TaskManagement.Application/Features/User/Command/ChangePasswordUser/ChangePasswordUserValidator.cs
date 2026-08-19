using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.User.Command.ChangePasswordUser;
internal class ChangePasswordUserValidator
    : AbstractValidator<ChangePasswordUserCommand>
{
    public ChangePasswordUserValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.OldPassword).NotEmpty().WithMessage("رمز عبور قدیمی نمیتواند خالی باشد!")
            .Length(8, 256).WithMessage("رمز عبور قدیمی باید بین 8 و 256 کاراکتر باشد!");

        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("رمز عبور نمیتواند خالی باشد!")
            .Length(8, 256).WithMessage("رمز عبور باید بین 8 و 256 کاراکتر باشد!")
            .NotEqual(x => x.ConfirmPassword).WithMessage("رمز عبور جدید و رمز عبور تایید برار نیست!");

        RuleFor(x => x.DeviceId).ValidateDeviceId();
    }
}