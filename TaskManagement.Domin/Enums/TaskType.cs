using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domin.Enums;
public enum TaskType
{
    [Display(Name = "منفرد")]
    Single,
    [Display(Name = "گروهی")]
    Group
}
