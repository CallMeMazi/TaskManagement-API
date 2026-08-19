using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Organization.Command.ChangeUserRoleToMember;
internal class ChangeUserRoleToMemberValidator
    : AbstractValidator<ChangeUserRoleToMemberCommand>
{
    public ChangeUserRoleToMemberValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.OrgOwnerId).ValidateId();

        RuleFor(x => x.OrgId).ValidateId();
    }
}
