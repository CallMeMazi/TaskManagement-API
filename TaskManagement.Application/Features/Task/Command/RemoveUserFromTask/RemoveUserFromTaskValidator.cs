using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Task.Command.RemoveUserFromTask;
public class RemoveUserFromTaskValidator
    : AbstractValidator<RemoveUserFromTaskCommand>
{
    public RemoveUserFromTaskValidator()
    {
        RuleFor(x => x.OwnerId).ValidateId();

        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.TaskId).ValidateId();

        RuleFor(x => x.ProjId).ValidateId();
    }
}
