using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Project.Command.UpdateProject;
public class UpdateProjectValidator
    : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.OwnerId).ValidateId();

        RuleFor(x => x.ProjId).ValidateId();

        RuleFor(x => x.ProjName).NotEmpty().WithMessage("نام پروژه نمیتواند خالی باشد!")
            .MaximumLength(50).WithMessage("نام پروژه نمیتواند بیشتر از 50 کاراکتر باشد!");

        RuleFor(x => x.ProjDescription).NotEmpty().WithMessage("توضیحات پروژه نمیتواند خالی باشد!")
            .MaximumLength(300).WithMessage("توضیحات پروژه نمیتواند بیشتز از 300 کاراکتر باشد!");
    }
}
