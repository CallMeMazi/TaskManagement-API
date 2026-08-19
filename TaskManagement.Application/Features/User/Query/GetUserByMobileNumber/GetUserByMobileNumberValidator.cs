using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.User.Query.GetUserByMobileNumber;
internal class GetUserByMobileNumberValidator
    : AbstractValidator<GetUserByMobileNumberQuery>
{
    public GetUserByMobileNumberValidator()
    {
        RuleFor(x => x.MobileNumber).ValidateMobileNumber();
    }
}
