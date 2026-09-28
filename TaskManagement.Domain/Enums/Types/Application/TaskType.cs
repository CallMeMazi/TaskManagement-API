using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Domain.Enums.Types.Application;
public enum TaskType
{
    [Display(Name = "منفرد")]
    Single,
    [Display(Name = "گروهی")]
    Group
}
