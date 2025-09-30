using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Enums.Roles;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class ProjectMemberShip : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid ProjId { get; private set; }
    public ProjectRoles Role { get; private set; }

    #region Navigation Prop

    public User User { get; private set; }
    public Project Project { get; private set; }

    #endregion


    private ProjectMemberShip() { }
    public ProjectMemberShip(Guid userId, Guid projId, ProjectRoles role)
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

    public void ValidateProjMemberShip(Guid userId, Guid projId)
    {
        var errorMessages = new List<string>();

        if (userId == Guid.Empty)
            errorMessages.Add("آیدی سازمان خالی است!");

        if (projId == Guid.Empty)
            errorMessages.Add("آیدی کاربر خالی است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعبر هستند!", errorMessages);
    }
}
