using TaskManagement.Common.Exceptions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domain.Enums.Roles;

namespace TaskManagement.Domain.Entities.BaseEntities;
public class OrganizationMemberShip : BaseEntity
{
    public long OrgId { get; private set; }
    public long UserId { get; private set; }
    public OrganizationRoles Role { get; private set; }

    #region Navigation Prop

    public Organization Org { get; private set; }
    public User User { get; private set; }

    #endregion


    private OrganizationMemberShip() { }
    public OrganizationMemberShip(long orgId, long userId, OrganizationRoles role)
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

    public void ValidateOrgMemberShip(long orgId, long userId)
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
