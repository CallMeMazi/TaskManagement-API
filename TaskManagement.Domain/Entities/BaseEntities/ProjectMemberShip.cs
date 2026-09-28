using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Enums.Roles;
using TaskManagement.Domain.Utilities.Exceptions;

namespace TaskManagement.Domain.Entities.BaseEntities;
public class ProjectMemberShip : BaseEntity
{
    public long UserId { get; private set; }
    public long ProjId { get; private set; }
    public ProjectRole Role { get; private set; }

    #region Navigation Prop

    public User User { get; private set; }
    public Project Project { get; private set; }

    #endregion


    private ProjectMemberShip() { }
    public ProjectMemberShip(long userId, long projId, ProjectRole role)
    {
        ValidateProjMemberShip(userId, projId);

        UserId = userId;
        ProjId = projId;
        Role = role;
    }


    public void ChangeUserOrgRole(ProjectRole role)
    {
        if (role == ProjectRole.Creator)
            throw new DomainLogicalException("نمیتوانید نقش کاربری را به سازنده تغییر دهید!");

        if (Role == role)
            throw new DomainLogicalException($"نقش کاربر در حال حاضر {role.ToDisplay()} است!");

        Role = role;

        UpdatedAt = DateTime.Now;
    }

    public void ValidateProjMemberShip(long userId, long projId)
    {
        var errorMessages = new List<string>();

        if (userId <= 0)
            errorMessages.Add("آیدی کاربر خالی است!");

        if (projId <= 0)
            errorMessages.Add("آیدی پروژه خالی است!");

        if (errorMessages.Any())
            throw new DomainValidationFailureException("اطلاعات نامعبر هستند!", errorMessages);
    }
}
