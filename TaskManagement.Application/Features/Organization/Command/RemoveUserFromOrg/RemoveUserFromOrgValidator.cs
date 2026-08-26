using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Organization.Command.RemoveUserFromOrg;
public class RemoveUserFromOrgValidator
    : AbstractValidator<RemoveUserFromOrgCommand>
{
    public RemoveUserFromOrgValidator()
    {
        RuleFor(x => x.OrgId).ValidateId();

        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.OrgOwnerId).ValidateId();
    }
}
