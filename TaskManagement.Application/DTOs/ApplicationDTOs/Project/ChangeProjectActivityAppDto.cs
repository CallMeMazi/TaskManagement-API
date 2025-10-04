namespace TaskManagement.Application.DTOs.ApplicationDTOs.Project;
public class ChangeProjectActivityAppDto
{
    public Guid UserId { get; set; }
    public Guid ProjId { get; set; }
    public bool Activity { get; set; }
}
