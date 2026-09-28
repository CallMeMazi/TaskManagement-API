using FluentValidation;
using TaskManagement.Application.Extentions;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Task.Command.CreateTask;
public class CreateTaskValidator
    : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.ProjId).ValidateId();

        RuleFor(x => x.TaskName).NotEmpty().WithMessage("نام تسک نمیتواند خالی باشد!")
            .MaximumLength(50).WithMessage("نام تسک نمیتواند بیشتر از 50 کاراکتر باشد!");

        RuleFor(x => x.TaskDescription).NotEmpty().WithMessage("توضیحات تسک نمیتواند خالی باشد!")
            .MaximumLength(150).WithMessage("توضیحات تسک نمیتواند بیشتر از 150 کاراکتر باشد!");

        RuleFor(x => x.TaskType).NotEmpty().WithMessage("مقدار نوع تسک نمیتواند خالی باشد!")
            .IsInEnum().WithMessage("مقدار نوع تسک نامعتبر است!");

        RuleFor(x => x.TaskDeadLine).NotEmpty().WithMessage("زمان پایان تسک نمیتواند خالی باشد!")
            .GreaterThan(DateTime.Now).WithMessage("زمان پایان تسک نامعتبر است!");

        When(x => x.UserIds is not null && x.UserIds.Any(), () =>
        {
            RuleFor(x => x.TaskType).NotEqual(TaskType.Single).WithMessage("تایپ این تسک مفرد و نمیتوان این تسک را به بیش از یک نفر الحاق کرد! ");

            RuleFor(x => x.UserIds).Must(x => x!.Distinct().Count() == x!.Count).WithMessage("آیدی تکراری در لیست وجود دارد!");

            RuleForEach(x => x.UserIds).ValidateId();
        });
    }
}
