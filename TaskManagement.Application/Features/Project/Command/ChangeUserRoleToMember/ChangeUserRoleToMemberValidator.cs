using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToMember;
public class ChangeUserRoleToMemberValidator
    : AbstractValidator<ChangeUserRoleToMemberCommand>
{
    public ChangeUserRoleToMemberValidator()
    {
        RuleFor(x => x.OwnerId).ValidateId();

        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.ProjId).ValidateId();
    }
}
