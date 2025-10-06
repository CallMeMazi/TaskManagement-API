using TaskManagement.Domin.Enums;
using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Application.DTOs.SharedDTOs.Task;
public class TaskDetailsDto
{
    public required string TaskName { get; set; }
    public required string TaskDescription { get; set; }
    public bool IsActive { get; set; }
    public TaskType TaskType { get; set; }
    public TaskStatusType TaskStatus { get; set; }
    public DateTime TaskDeadline { get; set; }
    public byte TaskProgress { get; set; }
    public DateTime CreateAt { get; set; }
}
