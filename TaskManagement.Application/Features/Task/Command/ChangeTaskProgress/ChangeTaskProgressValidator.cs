using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskProgress;
public class ChangeTaskProgressValidator
    : AbstractValidator<ChangeTaskProgressCommand>
{
    public ChangeTaskProgressValidator()
    {
        RuleFor(x => x.TaskId).ValidateId();

        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => (int)x.TaskProgress).NotEmpty().WithMessage("مقدار پیشرفت نمیتواند خالی باشد!")
            .InclusiveBetween(0, 100).WithMessage("مقدار پیشرفت باید بین 0 تا 100 باشد!");
    }
}
