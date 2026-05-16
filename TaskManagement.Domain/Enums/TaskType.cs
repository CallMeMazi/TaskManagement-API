using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Enums;
public enum TaskType
{
    [Display(Name = "منفرد")]
    Single,
    [Display(Name = "گروهی")]
    Group
}
