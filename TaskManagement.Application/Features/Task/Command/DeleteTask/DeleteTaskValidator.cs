using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Task.Command.DeleteTask;
public class DeleteTaskValidator
    : AbstractValidator<DeleteTaskCommand>
{
    public DeleteTaskValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.TaskId).ValidateId();
    }
}
