using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Task.Command.EndTask;
public class EndTaskValidator
    : AbstractValidator<EndTaskCommand>
{
    public EndTaskValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.TaskId).ValidateId();

        RuleFor(x => x.TaskInfoDescription).NotEmpty().WithMessage("توضیحات تسک نمیتواند خالی باشد!")
            .Length(6, 200).WithMessage("توضیحات تسک باید بین 6 تا 200 کاراکتر باشد!");
    }
}
