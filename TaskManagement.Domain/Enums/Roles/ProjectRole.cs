using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Enums.Roles;
public enum ProjectRole
{
    [Display(Name = "سازنده")]
    Creator,
    [Display(Name = "ادمین")]
    Admin,
    [Display(Name = "کاربر ساده")]
    Member
}
