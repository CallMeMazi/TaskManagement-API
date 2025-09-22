using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace TaskManagement.Domin.Enums;
public enum TaskType
{
    [Display(Name = "منفرد")]
    Single,
    [Display(Name = "گروهی")]
    Group
}
