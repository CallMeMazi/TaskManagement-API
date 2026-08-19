using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Organization.Command.LeaveUserFromOrg;
internal class LeaveUserFromOrgValidator
    : AbstractValidator<LeaveUserFromOrgCommand>
{
    public LeaveUserFromOrgValidator()
    {
        RuleFor(x => x.OrgId).ValidateId();

        RuleFor(x => x.UserId).ValidateId();
    }
}
