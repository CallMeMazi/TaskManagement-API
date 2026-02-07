namespace TaskManagement.Application.DTOs.ApplicationDTOs.Project;
public class ChangeProjectProgressAppDto
{
    public int OwnerId { get; set; }
    public int ProjId { get; set; }
    public byte ProjectProgress { get; set; }
}
