namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public class CreateTaskAppDto
{
    public Guid CreatorId { get; set; }
    public required string TaskName { get; set; }
    public required string TaskDescription { get; set; }
    public DateTime TaskDeadline { get; set; }
}
