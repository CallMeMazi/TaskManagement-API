using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domin.Enums;
public enum ProjectRoles
{
    [Display(Name = "سازنده")]
    Creator,
    [Display(Name = "ادمین")]
    Admin,
    [Display(Name = "کاربر ساده")]
    Member
}
