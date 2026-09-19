using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Task.Command.UpdateTask;
public class UpdateTaskValidator
    : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.TaskId).ValidateId();

        RuleFor(x => x.TaskName).NotEmpty().WithMessage("نام تسک نمیتواند خالی باشد!")
            .MaximumLength(50).WithMessage("نام تسک نمیتواند بیشتر از 50 کاراکتر باشد!");

        RuleFor(x => x.TaskDescription).NotEmpty().WithMessage("توضیحات تسک نمیتواند خالی باشد!")
            .MaximumLength(150).WithMessage("توضیحات تسک نمیتواند بیشتر از 150 کاراکتر باشد!");

        RuleFor(x => x.TaskDeadLine).NotEmpty().WithMessage("زمان پایان تسک نمیتواند خالی باشد!")
            .GreaterThan(DateTime.Now).WithMessage("زمان پایان تسک نامعتبر است!");
    }
}
