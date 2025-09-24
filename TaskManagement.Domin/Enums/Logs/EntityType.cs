using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domin.Enums.Logs;
public enum EntityType
{
    [Display(Name = "کاربر")]
    User,
    [Display(Name = "سازمان")]
    Organization,
    [Display(Name = "رابط کاربر و سازمان")]
    OrganizationMemberShip,
    [Display(Name = "پروژه")]
    Project,
    [Display(Name = "رابط کاربر و پروژه")]
    ProjectMemberShip,
    [Display(Name = "تسک")]
    Task,
    [Display(Name = "رابط گاربر و تسک")]
    TaskAssignment,
    [Display(Name = "توضیحات تسک")]
    TaskInfo,
    [Display(Name = "توکن")]
    UserToken
}
