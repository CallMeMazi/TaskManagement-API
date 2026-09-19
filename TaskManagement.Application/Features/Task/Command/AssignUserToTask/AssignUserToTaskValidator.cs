using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Task.Command.AssignUserToTask;
public class AssignUserToTaskValidator
    : AbstractValidator<AssignUserToTaskCommand>
{
    public AssignUserToTaskValidator()
    {
        RuleFor(x => x.OwnerId).ValidateId();

        RuleFor(x => x.TaskId).ValidateId();

        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.ProjId).ValidateId();
    }
}
