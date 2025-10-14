namespace TaskManagement.Application.DTOs.ApplicationDTOs.Task;
public class ChangeTaskActivityAppDto
{
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public bool Activity { get; set; }
}
