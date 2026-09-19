using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskType;
public class ChangeTaskTypeValidator
    : AbstractValidator<ChangeTaskTypeCommand>
{
    public ChangeTaskTypeValidator()
    {
        RuleFor(x => x.TaskId).ValidateId();

        RuleFor(x => x.UserId).ValidateId();
    }
}
