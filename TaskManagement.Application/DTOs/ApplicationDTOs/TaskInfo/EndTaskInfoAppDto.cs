namespace TaskManagement.Application.DTOs.ApplicationDTOs.TaskInfo;
public class EndTaskInfoAppDto
{
    public Guid UserId { get; set; }
    public Guid TaskInfoId { get; set; }
    public required string TaskInfoDescription { get; set; }
}
