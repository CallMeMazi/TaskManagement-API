namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public class ChangeTaskActivityAppDto
{
    public Guid UserId { get; set; }
    public Guid TaskId { get; set; }
    public bool Activity { get; set; }
}
