using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Enums.Roles;
public enum OrganizationRole
{
    [Display(Name = "مالک")]
    Owner,
    [Display(Name = "ادمین")]
    Admin,
    [Display(Name = "کاربر ساده")]
    Member
}
