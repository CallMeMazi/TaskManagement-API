using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public class ChangeTaskStatusAppDto : UserTaskAppDto
{
    public TaskStatusType TaskStatus { get; set; }
}
