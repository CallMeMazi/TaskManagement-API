using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Enums.Roles;
public enum ProjectRoles
{
    [Display(Name = "سازنده")]
    Creator,
    [Display(Name = "ادمین")]
    Admin,
    [Display(Name = "کاربر ساده")]
    Member
}
