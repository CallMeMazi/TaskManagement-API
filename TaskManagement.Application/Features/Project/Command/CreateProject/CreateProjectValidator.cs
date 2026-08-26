using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Project.Command.CreateProject;
public class CreateProjectValidator
    : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.ProjName).NotEmpty().WithMessage("نام پروژه نمیتواند خالی باشد!")
            .MaximumLength(50).WithMessage("نام پروژه نمیتواند بیشتر از 50 کاراکتر باشد!");

        RuleFor(x => x.ProjDescription).NotEmpty().WithMessage("توضیحات پروژه نمیتواند خالی باشد!")
            .MaximumLength(300).WithMessage("توضیحات پروژه نمیتواند بیشتز از 300 کاراکتر باشد!");

        RuleFor(x => x.CreatorId).ValidateId();

        RuleFor(x => x.OrgId).ValidateId();

        RuleFor(x => (int)x.MaxUser).NotEmpty().WithMessage("تعداد کارکنان نمیتواند خالی باشد!")
            .LessThanOrEqualTo(256).WithMessage("تعداد کارکنان نمیتواند بیشتر از 50 نفر باشد!");

        RuleFor(x => (int)x.MaxTask).NotEmpty().WithMessage("تعداد تسک نمیتواند خالی باشد!")
            .LessThanOrEqualTo(256).WithMessage("تعداد تسک نمیتواند بیشتر از 50 نفر باشد!");

        When(x => x.UserIds is not null && x.UserIds.Any(), () =>
        {
            RuleFor(x => x.UserIds).Must(x => x!.Distinct().Count() == x!.Count).WithMessage("آیدی تکراری در لیست وجود دارد!")
                .Must((x, ids) => ids!.Count == x.MaxUser).WithMessage("ایدی های وارد شده بیشتر از تعداد حداگثر کارکنان است!");

            RuleForEach(x => x.UserIds).ValidateId();
        });
    }
}
