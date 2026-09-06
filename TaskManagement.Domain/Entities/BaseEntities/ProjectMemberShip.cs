using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Enums.Roles;

namespace TaskManagement.Domain.Entities.BaseEntities;
public class ProjectMemberShip : BaseEntity
{
    public long UserId { get; private set; }
    public long ProjId { get; private set; }
    public ProjectRoles Role { get; private set; }

    #region Navigation Prop

    public User User { get; private set; }
    public Project Project { get; private set; }

    #endregion


    private ProjectMemberShip() { }
    public ProjectMemberShip(long userId, long projId, ProjectRoles role)
    {
        ValidateProjMemberShip(userId, projId);

        UserId = userId;
        ProjId = projId;
        Role = role;
    }


    public void ChangeUserOrgRole(ProjectRoles role)
    {
        if (role == ProjectRoles.Creator)
            throw new BadRequestException("نمیتوانید نقش کاربری را به سازنده تغییر دهید!");

        if (Role == role)
            throw new BadRequestException($"نقش کاربر در حال حاضر {role.ToDisplay()} است!");

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
            throw new BadRequestException("اطلاعات نامعبر هستند!", errorMessages);
    }
}
