using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Project.Command.AddUserToProject;
public class AddUserToProjectValidation
    : AbstractValidator<AddUserToProjectCommand>
{
    public AddUserToProjectValidation()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.OwnerId).ValidateId();

        RuleFor(x => x.ProjId).ValidateId();
    }
}
