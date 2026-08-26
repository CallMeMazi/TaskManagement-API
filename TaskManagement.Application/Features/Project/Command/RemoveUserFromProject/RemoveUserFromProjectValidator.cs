using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Project.Command.RemoveUserFromProject;
public class RemoveUserFromProjectValidator
    : AbstractValidator<RemoveUserFromProjectCommand>
{
    public RemoveUserFromProjectValidator()
    {
        RuleFor(x => x.ProjId).ValidateId();

        RuleFor(x => x.OwnerId).ValidateId();

        RuleFor(x => x.UserId).ValidateId();
    }
}
