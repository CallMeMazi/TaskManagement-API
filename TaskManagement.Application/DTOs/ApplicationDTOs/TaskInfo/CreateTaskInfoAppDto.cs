namespace TaskManagement.Application.DTOs.ApplicationDTOs.TaskInfo;
public class CreateTaskInfoAppDto
{
    public Guid TaskId { get; set; }
    public Guid UserId { get; set; }
    public Guid TaskAssignmentId { get; set; }
}
