namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public class UpdateTaskAppDto
{
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public required string TaskName { get; set; }
    public required string TaskDescription { get; set; }
    public DateTime TaskDeadline { get; set; }
}
