using FluentValidation;
using TaskManagement.Application.Extentions;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectProgress;
public class ChangeProjectProgressValidator
    : AbstractValidator<ChangeProjectProgressCommand>
{
    public ChangeProjectProgressValidator()
    {
        RuleFor(x => x.OwnerId).ValidateId();

        RuleFor(x => x.ProjId).ValidateId();

        RuleFor(x => (int)x.ProjectProgress).NotEmpty().WithMessage("")
            .InclusiveBetween(0, 100).WithMessage("مقدار پیشرفت باید بین 0 تا 100 باشد!");
    }
}
