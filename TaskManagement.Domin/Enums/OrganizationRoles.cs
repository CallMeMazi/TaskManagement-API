using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domin.Enums;
public enum OrganizationRoles
{
    [Display(Name = "مالک")]
    Owner,
    [Display(Name = "ادمین")]
    Admin,
    [Display(Name = "کاربر ساده")]
    Member
}
