using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Enums.Roles;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class ProjectMemberShip : BaseEntity
{
    public int UserId { get; private set; }
    public int ProjId { get; private set; }
    public ProjectRoles Role { get; private set; }

    #region Navigation Prop

    public User User { get; private set; }
    public Project Project { get; private set; }

    #endregion


    private ProjectMemberShip() { }
    public ProjectMemberShip(int userId, int projId, ProjectRoles role)
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

    public void ValidateProjMemberShip(int userId, int projId)
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
