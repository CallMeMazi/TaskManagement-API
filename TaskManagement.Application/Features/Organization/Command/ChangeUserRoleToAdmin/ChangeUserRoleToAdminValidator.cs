using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Organization.Command.ChangeUserRoleToAdmin;
internal class ChangeUserRoleToAdminValidator
    : AbstractValidator<ChangeUserRoleToAdminCommand>
{
    public ChangeUserRoleToAdminValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.OrgOwnerId).ValidateId();

        RuleFor(x => x.OrgId).ValidateId();
    }
}
