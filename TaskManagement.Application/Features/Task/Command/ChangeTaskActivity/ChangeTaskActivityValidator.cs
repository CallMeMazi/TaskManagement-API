using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskActivity;
public class ChangeTaskActivityValidator
    : AbstractValidator<ChangeTaskActivityCommand>
{
    public ChangeTaskActivityValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.TaskId).ValidateId();

        RuleFor(x => x.Activity).NotEmpty().WithMessage("مقدار فعال/غیرفعال نمیتواند خالی باشد!");
    }
}
