using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Organization.Command.CreateOrg;
public class CreateOrgValidator
    : AbstractValidator<CreateOrgCommand>
{
    public CreateOrgValidator()
    {
        RuleFor(x => x.OrgName).NotEmpty().WithMessage("نام سازمان نمیتواند خالی باشد!")
            .MaximumLength(100).WithMessage("نام سازمان نمیتواند بیشتر از 100 کاراکتر باشد!");

        RuleFor(x => x.SecondOrgName).NotEmpty().WithMessage("نام انگلیسی سازمان نمیتواند خالی باشد!")
            .MaximumLength(120).WithMessage("نام انکلیسی سازمان نمیتواند بیشتز از 120 کاراکتر باشد!");

        RuleFor(x => x.OrgDescription).NotEmpty().WithMessage("توضیحات سازمان نمیتواند خالی باشد!")
            .MaximumLength(400).WithMessage("توضیحات سازمان نمیتواند بیشتز از 120 کاراکتر باشد!");

        RuleFor(x => x.OwnerId).ValidateId();

        RuleFor(x => (int)x.MaxUser).NotEmpty().WithMessage("تعداد کارکنان نمیتواند خالی باشد!")
            .LessThanOrEqualTo(50).WithMessage("تعداد کارکنان نمیتواند بیشتر از 50 نفر باشد!");
    }
}
