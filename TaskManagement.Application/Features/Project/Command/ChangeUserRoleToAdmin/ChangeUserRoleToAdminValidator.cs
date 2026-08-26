using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToAdmin;
public class ChangeUserRoleToAdminValidator
    : AbstractValidator<ChangeUserRoleToAdminCommand>
{
    public ChangeUserRoleToAdminValidator()
    {
        RuleFor(x => x.OwnerId).ValidateId();

        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.ProjId).ValidateId();
    }
}
