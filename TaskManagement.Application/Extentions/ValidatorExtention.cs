using FluentValidation;
using TaskManagement.Common.Helpers;

namespace TaskManagement.Application.Extentions;
internal static class ValidatorExtention
{
    public static IRuleBuilderOptions<T, int> ValidateId<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder.NotNull().WithMessage($"شناسه نمیتواند خالی باشد")
            .LessThanOrEqualTo(0).WithMessage("شناسه نمیتوانید از صفر کمتر باشد!");
    }

    public static IRuleBuilderOptions<T, string> ValidateMobileNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().WithMessage("شماره موبایل نمیتواند خالی باشد!")
            .Length(11, 11).WithMessage("شماره موبایل باید 11 رقم باشد!")
            .Must(x => x.PhoneValid()).WithMessage("فرمت شماره موبایل نادرست است!");
    }

    public static IRuleBuilderOptions<T, string> ValidateEmail<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().WithMessage("ایمیل نمیتواند خالی باشد!")
            .MaximumLength(254).WithMessage("ایمیل باید کمتر از 254 نویسه باشد!")
            .EmailAddress().WithMessage("ایمیل نامعتبر است!")
            .Must(x => x.IsValidEmail()).WithMessage("ایمیل نامعتبر است!");
    }

    public static IRuleBuilderOptions<T, string> ValidateIp<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().WithMessage("Ip address can not be empty!")
            .Length(7, 50).WithMessage("The length of the IP address must be between 7 and 50!")
            .Must(x => x.IsValidIpAddress()).WithMessage("The ip address fromat is incorrect!");
    }

    public static IRuleBuilderOptions<T, string> ValidateDeviceId<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().WithMessage("The device id can not be empty!")
            .MaximumLength(50).WithMessage("The length of the device id must be less then 50!")
            .Matches("").WithMessage("The device id format is incorrect!");
    }

    public static IRuleBuilderOptions<T, string> ValidateUserAgent<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().WithMessage("The user agent can not be empty!")
            .MaximumLength(50).WithMessage("The length of the user agent must be less then 50!");
    }
}
