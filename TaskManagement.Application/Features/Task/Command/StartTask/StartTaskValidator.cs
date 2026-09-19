using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Task.Command.StartTask;
public class StartTaskValidator
    : AbstractValidator<StartTaskCommand>
{
    public StartTaskValidator()
    {
        RuleFor(x => x.TaskId).ValidateId();

        RuleFor(x => x.UserId).ValidateId();
    }
}
