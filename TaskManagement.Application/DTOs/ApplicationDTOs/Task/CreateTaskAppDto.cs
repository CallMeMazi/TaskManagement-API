using TaskManagement.Domin.Enums;

namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public class CreateTaskAppDto
{
    public Guid ProjId { get; set; }
    public Guid CreatorId { get; set; }
    public required string TaskName { get; set; }
    public required string TaskDescription { get; set; }
    public required TaskType TaskType { get; set; }
    public DateTime TaskDeadline { get; set; }
}
