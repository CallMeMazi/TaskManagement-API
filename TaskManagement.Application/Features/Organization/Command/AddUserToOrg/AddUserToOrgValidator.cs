using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Organization.Command.AddUserToOrg;
internal class AddUserToOrgValidator
    : AbstractValidator<AddUserToOrgCommand>
{
    public AddUserToOrgValidator()
    {
        RuleFor(x => x.UserId).ValidateId();

        RuleFor(x => x.OrgId).ValidateId();
    }
}
