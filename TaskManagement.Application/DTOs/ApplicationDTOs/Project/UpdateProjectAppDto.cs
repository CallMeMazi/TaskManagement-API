namespace TaskManagement.Application.DTOs.ApplicationDTOs.Project;
public class UpdateProjectAppDto
{
    public int ProjId { get; set; }
    public required string ProjName { get; set; }
    public required string ProjDescription { get; set; }
}
