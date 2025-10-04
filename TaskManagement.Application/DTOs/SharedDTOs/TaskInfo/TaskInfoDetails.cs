namespace TaskManagement.Application.DTOs.SharedDTOs.TaskInfo;
public class TaskInfoDetails
{
    public required string TaskInfoDescription { get; set; }
    public DateTime StartedTaskAt { get; set; }
    public DateTime EndedTaskAt { get; set; }
    public byte TotalHours { get; set; }
    public DateTime CreateAt { get; set; }
}
