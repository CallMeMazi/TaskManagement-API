using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Enums.Roles;

namespace TaskManagement.Domin.Entities.BaseEntities;
public class OrganizationMemberShip : BaseEntity
{
    public int OrgId { get; private set; }
    public int UserId { get; private set; }
    public OrganizationRoles Role { get; private set; }

    #region Navigation Prop

    public Organization Org { get; private set; }
    public User User { get; private set; }

    #endregion


    private OrganizationMemberShip() { }
    public OrganizationMemberShip(int orgId, int userId, OrganizationRoles role)
    {
        ValidateOrgMemberShip(orgId, userId);

        OrgId = orgId;
        UserId = userId;
        Role = role;
    }


    public void ChangeUserOrgRole(OrganizationRoles role)
    {
        if (role == OrganizationRoles.Owner)
            throw new BadRequestException("نمیتوانید نقش کاربری را به مالک تغییر دهید!");

        if (Role == role)
            throw new BadRequestException($"نقش کاربر در حال حاضر {role.ToDisplay()} است!");

        Role = role;

        UpdatedAt = DateTime.Now;
    }

    public void ValidateOrgMemberShip(int orgId, int userId)
    {
        var errorMessages = new List<string>();

        if (orgId <= 0)
            errorMessages.Add("آیدی سازمان خالی است!");

        if (userId <= 0)
            errorMessages.Add("آیدی کاربر خالی است!");

        if (errorMessages.Any())
            throw new BadRequestException("اطلاعات نامعبر هستند!", errorMessages);
    }
}
