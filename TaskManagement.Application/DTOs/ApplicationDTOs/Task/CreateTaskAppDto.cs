using TaskManagement.Domin.Enums;

namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public class CreateTaskAppDto
{
    public int ProjId { get; set; }
    public int CreatorId { get; set; }
    public required string TaskName { get; set; }
    public required string TaskDescription { get; set; }
    public required TaskType TaskType { get; set; }
    public DateTime TaskDeadline { get; set; }
}
