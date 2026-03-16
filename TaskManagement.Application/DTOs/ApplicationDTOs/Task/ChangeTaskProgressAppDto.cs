namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public class ChangeTaskProgressAppDto
{
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public byte TaskProgress { get; set; }
}
