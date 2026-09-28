using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Enums.Types.Logs;
public enum ActionType
{
    [Display(Name = "اضافه")]
    create,
    [Display(Name = "بروزرسانی")]
    Update,
    [Display(Name = "حذف")]
    Delete,
    [Display(Name = "اختصاص")]
    Assign
}
