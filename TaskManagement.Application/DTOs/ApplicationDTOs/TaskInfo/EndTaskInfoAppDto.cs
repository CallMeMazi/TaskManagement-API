namespace TaskManagement.Application.DTOs.ApplicationDTOs.TaskInfo;
public class EndTaskInfoAppDto
{
    public int UserId { get; set; }
    public int TaskInfoId { get; set; }
    public required string TaskInfoDescription { get; set; }
}
