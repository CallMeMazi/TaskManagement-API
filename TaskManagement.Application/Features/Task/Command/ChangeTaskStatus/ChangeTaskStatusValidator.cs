using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskStatus;
public class ChangeTaskStatusValidator
    : AbstractValidator<ChangeTaskStatusCommand>
{
    public ChangeTaskStatusValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.TaskId).ValidateId();

        RuleFor(x => x.TaskStatus).NotEmpty().WithMessage("مقدار وضعیت تسک نمیتواند خالی باشد!")
            .IsInEnum().WithMessage("مقدار وضعیت تسک نامعتبر است!");
    }
}
